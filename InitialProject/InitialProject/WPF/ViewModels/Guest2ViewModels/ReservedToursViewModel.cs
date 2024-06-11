using ceTe.DynamicPDF.PageElements;
using ceTe.DynamicPDF;
using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.DTOs;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Security.Cryptography.Xml;
using System.Windows.Threading;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class ReservedToursViewModel : ViewModelBase
    {
        #region PROPERTIES
        public User LoggedUser { get; set; }

        private TourCheckpoint? _selectedReservedTour;
        public TourCheckpoint? SelectedReservedTour
        {
            get
            {
                return _selectedReservedTour;
            }
            set
            {
                if (_selectedReservedTour != value)
                {
                    _selectedReservedTour = value;
                    OnPropertyChanged(nameof(SelectedReservedTour));
                }
            }
        }
        private bool _isEnabled;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged(nameof(IsEnabled));
                }
            }
        }

        private DateTime _dateFrom;
        public DateTime DateFrom
        {
            get
            {
                return _dateFrom;
            }
            set
            {
                if (_dateFrom != value)
                {
                    _dateFrom = value;
                    OnPropertyChanged(nameof(DateFrom));
                }
            }
        }

        private DateTime _dateTo;
        public DateTime DateTo
        {
            get
            {
                return _dateTo;
            }
            set
            {
                if (_dateTo != value)
                {
                    _dateTo = value;
                    OnPropertyChanged(nameof(DateTo));
                }
            }
        }

        private string _demoButtonContent;
        public string DemoButtonContent
        {
            get
            {
                return _demoButtonContent;
            }
            set
            {
                if (_demoButtonContent != value)
                {
                    _demoButtonContent = value;
                    OnPropertyChanged(nameof(DemoButtonContent));
                }
            }
        }

        public DispatcherTimer Timer { get; set; }

        public ObservableCollection<TourCheckpoint> ReservedTours { get; set; }
        private readonly TourService _tourService;
        private readonly TourReviewService _tourReviewService;
        private readonly TourReservationService _tourReservationService;
        #endregion

        public ReservedToursViewModel(User user)
        {
            _tourService = new TourService();
            LoggedUser = user;
            ReservedTours = new ObservableCollection<TourCheckpoint>();
            _tourReviewService = new TourReviewService();
            _tourReservationService = new TourReservationService();

            DemoButtonContent = "Demo";

            HomeCommand = new RelayCommand(HomeCommand_Execute);
            OpenRateTourAndGuideWindowCommand = new RelayCommand(OpenRateTourAndGuideWindowCommand_Execute);
            //IsEnabled = false;
            GeneratePDFCommand = new RelayCommand(GeneratePDFCommand_Execute);
            DemoCommand = new RelayCommand(DemoCommand_Execute);

            LoadReservedTours();
        }

        public void LoadReservedTours()
        {
            foreach(var reservation in _tourService.GetReservedTours(LoggedUser))
            {
                ReservedTours.Add(reservation);
            }
        }

        public bool HasSelectedValidReservation()
        {
            if(SelectedReservedTour != null && SelectedReservedTour.Status == TourStatus.FINISHED)
            {
                return true;
            }
            return false;
        }



        public bool IsAlreadyRated(int tourId, int userId)
        {
            return _tourReviewService.HasAlreadyBeenRated(tourId, userId);
        }

        public void GeneratePDF(DateTime fromDate, DateTime toDate, User user, List<TourReservation> reservedTours)
        {
            Document document = new Document();
            Page page = new Page(PageSize.Letter, PageOrientation.Portrait, 45.0f);
            document.Pages.Add(page);

            string title = "Tours That You Attended";
            Label label = new Label(title, 0, 0, 504, 100, Font.TimesBold, 30, TextAlign.Center);
            page.Elements.Add(label);

            List<Tour> tours = new List<Tour>();
            foreach(var reservation in reservedTours)
            {
                if(reservation.UserId == user.Id)
                {
                    tours.Add(_tourService.GetById(reservation.TourId));
                }
            }

            foreach(var tour in tours.ToList())
            {
                if(DateTime.Compare(fromDate, tour.StartTime) > 0 || DateTime.Compare(toDate, tour.StartTime.AddHours(tour.Duration)) < 0)
                {
                    tours.Remove(tour);
                }
            }

            Table2 table = new Table2(0, 50, 600, 600);

            Column2 column1 = table.Columns.Add(40);
            column1.CellDefault.Align = TextAlign.Center;
            table.Columns.Add(70);
            table.Columns.Add(140);
            table.Columns.Add(100);
            table.Columns.Add(100);

            Row2 row1 = table.Rows.Add(40, Font.TimesBold, 16, Grayscale.Black,
               Grayscale.Gray);
            row1.CellDefault.Align = TextAlign.Center;
            row1.CellDefault.VAlign = VAlign.Center;
            row1.Cells.Add("");
            row1.Cells.Add("Tour Name");
            row1.Cells.Add("Started On:");
            row1.Cells.Add("Language:");
            row1.Cells.Add("Duration(h):");

            int i = 1;
            foreach (var tour in tours)
            {
                Row2 row2 = table.Rows.Add(30);
                Cell2 cell1 = row2.Cells.Add($"{i}", Font.HeiseiKakuGothicW5, 16,
                    Grayscale.Black, Grayscale.Gray, 1);
                cell1.Align = TextAlign.Center;
                cell1.VAlign = VAlign.Center;
                row2.Cells.Add($"{tour.Name}");
                row2.Cells.Add($"{tour.StartTime}");
                row2.Cells.Add($"{tour.Language}");
                row2.Cells.Add($"{tour.Duration}");
                i++;
            }

            table.CellDefault.Padding.Value = 5.0f;
            table.CellSpacing = 5.0f;
            table.Border.Top.Color = RgbColor.Violet;
            table.Border.Bottom.Color = RgbColor.Violet;
            table.Border.Top.Width = 2;
            table.Border.Bottom.Width = 2;
            table.Border.Left.LineStyle = LineStyle.None;
            table.Border.Right.LineStyle = LineStyle.None;

            page.Elements.Add(table);
            

            string guestName = $"Guest: {LoggedUser.Username}";
            Label guestNameLabel = new Label(guestName, 0, 700, 504, 100, Font.TimesRoman, 20, TextAlign.Right);
            page.Elements.Add(guestNameLabel);

            document.Draw($"../../../Reports/toursReport.pdf");

            string absolutePath = AppDomain.CurrentDomain.BaseDirectory + $"../../../Reports/toursReport.pdf";

            Process.Start(new ProcessStartInfo
            {
                FileName = absolutePath,
                UseShellExecute = true
            });
        }

        private void RunDemo()
        {
            var demoDateFrom = DateTime.Parse("1.1.2019.");
            var demoDateTo = DateTime.Parse("1.1.2024.");

            Timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(0.2)
            };

            this.ResetFields();
            int i = 0;

            Timer.Tick += delegate (object? sender, EventArgs e)
            {
                if (i < 5)
                {
                    DateFrom = demoDateFrom;
                }
                else if (i < 25)
                {
                    DateTo = demoDateTo;
                }
                else if (i < 35)
                {
                    this.GeneratePDFCommand_Execute(null);
                    StopDemo();
                }
                else
                {
                    Timer.Stop();
                }
                i++;
            };
            Timer.Start();
            DemoButtonContent = "Stop";
        }

        public void StopDemo()
        {
            DemoButtonContent = "Demo";
            Timer.Stop();
            ResetFields();
        }

        private void ResetFields()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
        }

        #region COMMANDS

        public RelayCommand HomeCommand { get; }
        public RelayCommand OpenRateTourAndGuideWindowCommand { get; }
        public RelayCommand GeneratePDFCommand { get; }
        public RelayCommand DemoCommand { get; }

        public void DemoCommand_Execute(object? parameter)
        {
            if (DemoButtonContent.Equals("Demo"))
            {
                RunDemo();
            }
            else
            {
                StopDemo();
            }
        }

        public void GeneratePDFCommand_Execute(object? parameter)
        {
            GeneratePDF(DateFrom, DateTo, LoggedUser, _tourReservationService.GetAll());
        }

        public void HomeCommand_Execute(object? parameter)
        {
            Guest2TourView guest2TourView = new Guest2TourView(LoggedUser);
            //_reservedToursView.Close();
        }
        public void OpenRateTourAndGuideWindowCommand_Execute(object? parameter)
        {
            if (!HasSelectedValidReservation())
            {
                MessageBox.Show("Selected tour has to be FINISHED in order to be rated.");
            }
            else if (IsAlreadyRated(SelectedReservedTour.TourId, LoggedUser.Id))
            {
                MessageBox.Show("This tour has already been rated.");
            }
            else
            {
                RateTourAndGuideForm rateTourAndGuideForm = new RateTourAndGuideForm(SelectedReservedTour.TourId, LoggedUser);
                //rateTourAndGuideForm.Show();
                //_reservedToursView.Close();
            }
        }
        #endregion
    }
}
