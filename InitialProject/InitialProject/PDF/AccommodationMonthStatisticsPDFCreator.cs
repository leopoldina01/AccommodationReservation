using InitialProject.Application.UseCases;
using InitialProject.Domain.Models;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Drawing;
using System.Globalization;

namespace InitialProject.PDF
{
    public class AccommodationMonthStatisticsPDFCreator
    {
        private AccommodationMonthStatisticsService _accommodationMonthStatisticsService;
        private AccommodationYearStatistic _selectedYearStatistic;
        private Accommodation _selectedAccommodation;
        private User _owner;

        public AccommodationMonthStatisticsPDFCreator (AccommodationMonthStatisticsService accommodationMonthStatisticsService, AccommodationYearStatistic selectedYearStatistic, Accommodation selectedAccommodation)
        {
            _accommodationMonthStatisticsService = accommodationMonthStatisticsService;
            _selectedYearStatistic = selectedYearStatistic;
            _selectedAccommodation = selectedAccommodation;
        }

        public void CreatePDF(User owner)
        {
            try
            {
                IEnumerable<AccommodationMonthStatistics> monthStatistics = _accommodationMonthStatisticsService.GetAllByYearStatistic(_selectedYearStatistic.Id);
                PdfDocument document = new PdfDocument();
                DrawAllGrids(document, monthStatistics, owner);
                FileStream stream = CreateAndSaveDocument(document);
                CloseStreams(stream, document);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void CloseStreams(FileStream stream, PdfDocument document)
        {
            stream.Close();
            document.Close(true);
        }

        private FileStream CreateAndSaveDocument(PdfDocument document)
        {
            FileStream stream = File.Create($"../../../Reports/monthStatistics.pdf");
            document.Save(stream);
            return stream;
        }

        private void DrawAllGrids(PdfDocument document, IEnumerable<AccommodationMonthStatistics> monthStatistics, User owner)
        {
            PdfPage page = document.Pages.Add();

            if (monthStatistics.Count() == 0)
            {
                return;
            }

            PdfGraphics graphics = page.Graphics;

            graphics.DrawRectangle(new PdfPen(PdfBrushes.DeepSkyBlue, 10), new RectangleF(0, 0, page.Size.Width, page.Size.Height));

            PdfLayoutResult result = CreateGrid(page, monthStatistics);

            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16);
            PdfFont contentFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

            float xPosition = 10;
            float yPosition = 10;

            string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            graphics.DrawString("Date and Time: " + dateTime, contentFont, PdfBrushes.Black, new PointF(page.Size.Width - 300, yPosition));
            yPosition += contentFont.Size + 5;

            graphics.DrawString("Created by " + owner.Username, contentFont, PdfBrushes.Black, new PointF(page.Size.Width - 300, yPosition));
            yPosition += contentFont.Size + 5;

            graphics.DrawString("Accommodation name: " + _selectedAccommodation.Name, titleFont, PdfBrushes.Black, new PointF(xPosition, yPosition));
            yPosition += titleFont.Size + 5;

            graphics.DrawString("Country: " + _selectedAccommodation.Location.Country, contentFont, PdfBrushes.Black, new PointF(xPosition, yPosition));
            yPosition += contentFont.Size + 5;

            graphics.DrawString("City: " + _selectedAccommodation.Location.City, contentFont, PdfBrushes.Black, new PointF(xPosition, yPosition));
            yPosition += contentFont.Size + 5;

            graphics.DrawString("Type: " + _selectedAccommodation.Type, contentFont, PdfBrushes.Black, new PointF(xPosition, yPosition));
            yPosition += contentFont.Size + 10;

            graphics.DrawString("Year: " + _selectedYearStatistic.Year, contentFont, PdfBrushes.Black, new PointF(xPosition, yPosition));
            yPosition += contentFont.Size + 10;

            PdfFont titleFont2 = new PdfStandardFont(PdfFontFamily.Helvetica, 18, PdfFontStyle.Bold);
            graphics.DrawString("Accommodation statistics", titleFont2, PdfBrushes.Black, new PointF(xPosition, yPosition));

            result.Page.Graphics.DrawString("Accommodation statistics", new PdfStandardFont(PdfFontFamily.Helvetica, 10), PdfBrushes.Black, new PointF(210, 0));
        }

        private PdfLayoutResult CreateGrid(PdfPage page, IEnumerable<AccommodationMonthStatistics> monthStatistics)
        {
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            layoutFormat.Layout = PdfLayoutType.Paginate;
            PdfGrid firstGrid = CreateGridFromStatistics(monthStatistics);
            PdfLayoutResult result = firstGrid.Draw(page, new PointF(10, 155), layoutFormat);
            return result;
        }

        private PdfGrid CreateGridFromStatistics(IEnumerable<AccommodationMonthStatistics> monthStatistics)
        {
            PdfGrid pdfGrid = new PdfGrid();
            DataTable table = CreateDataTableForStatistics(monthStatistics);

            pdfGrid.Style.CellPadding.All = 5;
            pdfGrid.Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

            pdfGrid.Style.BackgroundBrush = PdfBrushes.LightBlue;

            pdfGrid.DataSource = table;

            return pdfGrid;
        }

        private DataTable CreateDataTableForStatistics(IEnumerable<AccommodationMonthStatistics> monthStatistics)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Month");
            dataTable.Columns.Add("Number of reservations");
            dataTable.Columns.Add("Declined reservations");
            dataTable.Columns.Add("Changed reservations");
            dataTable.Columns.Add("Renovation suggestions");

            foreach (AccommodationMonthStatistics statistic in monthStatistics)
            {
                dataTable.Rows.Add(new object[] { ConvertMonthToString(statistic.Month), statistic.NumberOfReservations.ToString(), statistic.NumberOfDeclinedReservations.ToString(), statistic.NumberOfChangedReservations.ToString(), statistic.NumberOfRenovationSuggestions.ToString() });
            }

            return dataTable;
        }

        private String ConvertMonthToString(int month)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
        }
    }
}
