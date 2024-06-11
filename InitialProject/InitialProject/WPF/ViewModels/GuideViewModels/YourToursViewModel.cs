using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace InitialProject.WPF.ViewModels
{
    public class YourToursViewModel : ViewModelBase
    {
        #region PROPERTIES

        private Tour? _selectedPastTour;
        public Tour? SelectedPastTour
        {
            get
            {
                return _selectedPastTour;
            }
            set
            {
                if (_selectedPastTour != value)
                {
                    _selectedPastTour = value;
                    OnPropertyChanged(nameof(SelectedPastTour));
                }
            }
        }

        private Tour? _selectedFutureTour;
        public Tour? SelectedFutureTour
        {
            get
            {
                return _selectedFutureTour;
            }
            set
            {
                if (_selectedFutureTour != value)
                {
                    _selectedFutureTour = value;
                    OnPropertyChanged(nameof(SelectedFutureTour));

                }
            }
        }

        public ObservableCollection<Tour> PastTours { get; set; }
        public ObservableCollection<Tour> FutureTours { get; set; }

        private readonly TourService _tourService;
        private readonly TourReservationService _tourReservationService;
        private readonly CheckpointArrivalService _checkpointArrivalService;

        private readonly User _guide;

        private readonly Stack<Tuple<ReversibleCommand, object?>> _commandStack;
        #endregion

        public YourToursViewModel(User guide, Stack<Tuple<ReversibleCommand, object?>> commandStack)
        {
            _guide = guide;
            _commandStack = commandStack;

            _tourService = new TourService();
            _tourReservationService = new TourReservationService();
            _checkpointArrivalService = new CheckpointArrivalService();

            PastTours = new ObservableCollection<Tour>();
            FutureTours = new ObservableCollection<Tour>();

            CancelTourCommand = new ReversibleCommand(CancelTourCommand_Execute, CancelTourCommand_CanExecute, CancelTourCommand_Reverse);
            ReportCommand = new RelayCommand(ReportCommand_Execute);

            LoadFutureTours();
            LoadPastTours();
        }
        public void LoadFutureTours()
        {
            FutureTours.Clear();
            foreach (var tour in _tourService.GetFutureToursByGuide(_guide))
            {
                FutureTours.Add(tour);
            }
        }

        public void LoadPastTours()
        {
            PastTours.Clear();
            foreach (var tour in _tourService.GetPastToursByGuide(_guide))
            {
                PastTours.Add(tour);
            }
        }

        private void GeneratePdfReport(Tour? tour)
        {
            Document document = new Document();
            Page page = new Page(PageSize.Letter, PageOrientation.Portrait, 45.0f);
            document.Pages.Add(page);

            string title = "Tour \"" + tour.Name + "\" guest list:";
            Label label = new Label(title, 0, 0, 504, 100, Font.TimesBold, 30, TextAlign.Center);
            page.Elements.Add(label);

            Table2 table = new Table2(0, 50, 600, 600);

            Column2 column1 = table.Columns.Add(40);
            column1.CellDefault.Align = TextAlign.Center;
            table.Columns.Add(150);
            table.Columns.Add(150);
            table.Columns.Add(150);

            Row2 row1 = table.Rows.Add(40, Font.TimesBold, 16, Grayscale.Black,
               Grayscale.Gray);
            row1.CellDefault.Align = TextAlign.Center;
            row1.CellDefault.VAlign = VAlign.Center;
            row1.Cells.Add("");
            row1.Cells.Add("Guest");
            row1.Cells.Add("Arrived at");
            row1.Cells.Add("Number of people");

            int i = 1;
            foreach (var arrival in _checkpointArrivalService.GetAllByTour(tour))
            {
                Row2 row2 = table.Rows.Add(30);
                Cell2 cell1 = row2.Cells.Add($"{i}", Font.HelveticaBold, 16,
                   Grayscale.Black, Grayscale.Gray, 1);
                cell1.Align = TextAlign.Center;
                cell1.VAlign = VAlign.Center;
                row2.Cells.Add($"{arrival.Reservation.User.Username}");
                row2.Cells.Add($"{arrival.Checkpoint.Name}");
                row2.Cells.Add($"{arrival.Reservation.NumberOfPeople}");
                i++;
            }


            table.CellDefault.Padding.Value = 5.0f;
            table.CellSpacing = 5.0f;
            table.Border.Top.Color = RgbColor.Blue;
            table.Border.Bottom.Color = RgbColor.Blue;
            table.Border.Top.Width = 2;
            table.Border.Bottom.Width = 2;
            table.Border.Left.LineStyle = LineStyle.None;
            table.Border.Right.LineStyle = LineStyle.None;

            page.Elements.Add(table);

            string guideName = $"Guide: {_guide.Username}";
            Label guideNameLabel = new Label(guideName, 0, 630, 504, 100, Font.TimesRoman, 20, TextAlign.Right);
            page.Elements.Add(guideNameLabel);

            Line line = new Line(380, 700, 530, 700, 1, Grayscale.Black,
           LineStyle.Solid);

            page.Elements.Add(line);

            document.Draw($"../../../Reports/{tour.Name}.pdf");

            string absolutePath = AppDomain.CurrentDomain.BaseDirectory + $"../../../Reports/{tour.Name}.pdf";

            Process.Start(new ProcessStartInfo
            {
                FileName = absolutePath,
                UseShellExecute = true
            });
        }

        #region COMMANDS
        public ReversibleCommand CancelTourCommand { get; }
        public RelayCommand ReportCommand { get; }
        public void CancelTourCommand_Execute(object? parameter)
        {
            if (MessageBox.Show("Are you sure you want to cancel this tour? You can undo this action later but all the reservations will remain deleted and guests will keep their vouchers!", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No) return;
            _tourService.CancelTour(SelectedFutureTour);
            _tourReservationService.DeleteAllReservationsForCancelledTour(SelectedFutureTour, _guide);
            LoadFutureTours();

            _commandStack.Push(new Tuple<ReversibleCommand, object?>(CancelTourCommand, parameter));
        }

        public bool CancelTourCommand_CanExecute(object? parameter)
        {
            Tour? tour = parameter as Tour;
            return tour is not null && tour.Status != TourStatus.CANCELED && tour.StartTime.Subtract(DateTime.Now) > TimeSpan.FromDays(2);
        }

        public void CancelTourCommand_Reverse(object? parameter)
        {
            Tour? tour = parameter as Tour;
            _tourService.UncancelTour(tour);
            LoadFutureTours();
        }

        public void ReportCommand_Execute(object? parameter)
        {
            var tour = parameter as Tour;
            GeneratePdfReport(tour);
        }
        #endregion
    }
}
