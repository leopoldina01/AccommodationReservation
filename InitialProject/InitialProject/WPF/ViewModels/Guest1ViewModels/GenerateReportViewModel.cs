using ceTe.DynamicPDF;
using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ceTe.DynamicPDF.PageElements;
using ceTe.DynamicPDF;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class GenerateReportViewModel : ViewModelBase
    {
        #region PROPERTIES
        private DateTime _selectedStartDate;
        public DateTime SelectedStartDate
        {
            get
            {
                return _selectedStartDate;
            }
            set
            {
                if (_selectedStartDate != value)
                {
                    _selectedStartDate = value;
                    OnPropertyChanged(nameof(SelectedStartDate));
                }
            }
        }
        private DateTime _selectedEndDate;
        public DateTime SelectedEndDate
        {
            get
            {
                return _selectedEndDate;
            }
            set
            {
                if (_selectedEndDate != value)
                {
                    _selectedEndDate = value;
                    OnPropertyChanged(nameof(SelectedEndDate));
                }
            }
        }
        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public User LoggedUser { get; set; }

        private readonly AccommodationReservationService _accommodationReservationService;
        #endregion
        public GenerateReportViewModel(User user)
        {
            _accommodationReservationService = new AccommodationReservationService();

            Status = "Scheduled";
            LoggedUser = user;

            GenerateReportCommand = new RelayCommand(GenerateReportCommand_Execute);
        }

        #region COMMANDS
        public RelayCommand GenerateReportCommand { get; }
        public void GenerateReportCommand_Execute(object? parameter)
        {
            Document document = new Document();
            Page page = new Page(PageSize.Letter, PageOrientation.Portrait, 45.0f);
            document.Pages.Add(page);

            string title = "All " + Status.ToLower() + " reservations from " + SelectedStartDate.ToString("dd/M/yyyy") + " to " + SelectedEndDate.ToString("dd/M/yyyy");
            Label label = new Label(title, 0, 0, 504, 100, Font.TimesBold, 20, TextAlign.Center);
            page.Elements.Add(label);

            Table2 table = new Table2(-20, 50, 600, 600);

            Column2 column1 = table.Columns.Add(40);
            column1.CellDefault.Align = TextAlign.Center;
            table.Columns.Add(80);
            table.Columns.Add(80);
            table.Columns.Add(80);
            table.Columns.Add(80);
            table.Columns.Add(80);
            table.Columns.Add(80);

            Row2 row1 = table.Rows.Add(40, Font.TimesBold, 12, Grayscale.Black,
               Grayscale.Gray);
            row1.CellDefault.Align = TextAlign.Center;
            row1.CellDefault.VAlign = VAlign.Center;
            row1.Cells.Add("");
            row1.Cells.Add("Name");
            row1.Cells.Add("Country");
            row1.Cells.Add("City");
            row1.Cells.Add("Type");
            row1.Cells.Add("Min days before cancel");
            row1.Cells.Add("Date");

            int i = 1;
            if (Status.Equals("Scheduled"))
            {
                foreach (var reservation in _accommodationReservationService.GetGuestsReservations(LoggedUser.Id))
                {
                    if (DateTime.Compare(reservation.StartDate, SelectedStartDate) >= 0 && DateTime.Compare(reservation.EndDate, SelectedEndDate) <= 0)
                    {
                        Row2 row2 = table.Rows.Add(30);
                        Cell2 cell1 = row2.Cells.Add($"{i}", Font.HelveticaBold, 12,
                           Grayscale.Black, Grayscale.Gray, 1);
                        cell1.Align = TextAlign.Center;
                        cell1.VAlign = VAlign.Center;
                        row2.Cells.Add($"{reservation.Accommodation.Name}");
                        row2.Cells.Add($"{reservation.Accommodation.Location.Country}");
                        row2.Cells.Add($"{reservation.Accommodation.Location.City}");
                        row2.Cells.Add($"{reservation.Accommodation.Type}");
                        row2.Cells.Add($"{reservation.Accommodation.MinDaysBeforeCancel}");
                        row2.Cells.Add($"{reservation.StartDate.ToString("dd/M/yyyy")} - {reservation.EndDate.ToString("dd/M/yyyy")}");
                        i++;
                    }
                }
            }
            else
            {
                foreach (var reservation in _accommodationReservationService.GetGuestsCancelledReservations(LoggedUser.Id))
                {
                    if (DateTime.Compare(reservation.StartDate, SelectedStartDate) >= 0 && DateTime.Compare(reservation.EndDate, SelectedEndDate) <= 0)
                    {
                        Row2 row2 = table.Rows.Add(30);
                        Cell2 cell1 = row2.Cells.Add($"{i}", Font.HelveticaBold, 12,
                           Grayscale.Black, Grayscale.Gray, 1);
                        cell1.Align = TextAlign.Center;
                        cell1.VAlign = VAlign.Center;
                        row2.Cells.Add($"{reservation.Accommodation.Name}");
                        row2.Cells.Add($"{reservation.Accommodation.Location.Country}");
                        row2.Cells.Add($"{reservation.Accommodation.Location.City}");
                        row2.Cells.Add($"{reservation.Accommodation.Type}");
                        row2.Cells.Add($"{reservation.Accommodation.MinDaysBeforeCancel}");
                        row2.Cells.Add($"{reservation.StartDate.ToString("dd/M/yyyy")} - {reservation.EndDate.ToString("dd/M/yyyy")}");
                        i++;
                    }
                }
            }
            
            table.CellDefault.Padding.Value = 5.0f;
            table.CellSpacing = 5.0f;
            table.Border.Top.Color = RgbColor.Green;
            table.Border.Bottom.Color = RgbColor.Green;
            table.Border.Top.Width = 2;
            table.Border.Bottom.Width = 2;
            table.Border.Left.LineStyle = LineStyle.None;
            table.Border.Right.LineStyle = LineStyle.None;

            page.Elements.Add(table);

            string guestName = $"Guest: {LoggedUser.Username}";
            Label guestNameLabel = new Label(guestName, 0, 700, 504, 30, Font.TimesRoman, 20, TextAlign.Right);
            page.Elements.Add(guestNameLabel);

            //string currentTime = $"{DateTime.Now}";
            //Label currentTimeLabel = new Label(currentTime, 200, 600, 100, 30, Font.TimesRoman, 18, TextAlign.Right);
            //page.Elements.Add(currentTimeLabel);

            document.Draw("../../../Reports/reservationsReport.pdf");

            string absolutePath = AppDomain.CurrentDomain.BaseDirectory + "../../../Reports/reservationsReport.pdf";

            Process.Start(new ProcessStartInfo
            {
                FileName = absolutePath,
                UseShellExecute = true
            });
        }

        public bool GenerateReportCommand_CanExecute(object? parameter)
        {
            return SelectedStartDate != null && SelectedEndDate != null;
        }
        #endregion
    }
}
