using InitialProject.Application.UseCases;
using InitialProject.Domain.Models;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.PDF
{
    public class AccommodationYearStatisticsPDFCreator
    {
        private readonly AccommodationYearStatisticsService _accommodationYearStatisticsService;
        private Accommodation _selectedAccommodation;
        private User _owner;

        public AccommodationYearStatisticsPDFCreator(AccommodationYearStatisticsService accommodationYearStatisticsService, Accommodation selectedAccommodation)
        {
            _accommodationYearStatisticsService = accommodationYearStatisticsService;
            _selectedAccommodation = selectedAccommodation;
        }

        public void CreatePDF(User owner)
        {
            try
            {
                List<AccommodationYearStatistic> yearStatistics = _accommodationYearStatisticsService.GetAllByAccommodationId(_selectedAccommodation.Id);
                PdfDocument document = new PdfDocument();
                _owner = owner;
                DrawAllGrids(document, yearStatistics, owner);
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
            FileStream stream = File.Create($"../../../Reports/yearStatistics.pdf");
            document.Save(stream);
            return stream;
        }

        private void DrawAllGrids(PdfDocument document, List<AccommodationYearStatistic> yearStatistics, User owner)
        {
            PdfPage page = document.Pages.Add();

            if (yearStatistics.Count == 0)
            {
                return;
            }

            PdfGraphics graphics = page.Graphics;

            graphics.DrawRectangle(new PdfPen(PdfBrushes.DeepSkyBlue, 10), new RectangleF(0, 0, page.Size.Width, page.Size.Height));

            PdfLayoutResult result = CreateGrid(page, yearStatistics);

            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Bold);
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

            PdfFont titleFont2 = new PdfStandardFont(PdfFontFamily.Helvetica, 18, PdfFontStyle.Bold);
            graphics.DrawString("Accommodation statistics", titleFont2, PdfBrushes.Black, new PointF(xPosition, yPosition));

            yPosition += titleFont2.Size + 10;

            result.Page.Graphics.DrawString("Accommodation statistics", new PdfStandardFont(PdfFontFamily.Helvetica, 10), PdfBrushes.Black, new PointF(210, 0));
        }

        private PdfLayoutResult CreateGrid(PdfPage page, List<AccommodationYearStatistic> yearStatistics)
        {
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            layoutFormat.Layout = PdfLayoutType.Paginate;
            PdfGrid firstGrid = CreateGridFromStatistic(yearStatistics);
            PdfLayoutResult result = firstGrid.Draw(page, new PointF(10, 140), layoutFormat);
            return result;
        }

        private PdfGrid CreateGridFromStatistic(List<AccommodationYearStatistic> yearStatistics)
        {
            PdfGrid pdfGrid = new PdfGrid();
            DataTable table = CreateDataTableForStatistics(yearStatistics);

            pdfGrid.Style.CellPadding.All = 5;
            pdfGrid.Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

            pdfGrid.Style.BackgroundBrush = PdfBrushes.LightBlue;

            pdfGrid.DataSource = table;

            return pdfGrid;
        }

        private DataTable CreateDataTableForStatistics(List<AccommodationYearStatistic> statistics)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Year");
            dataTable.Columns.Add("Number of reservations");
            dataTable.Columns.Add("Declined reservations");
            dataTable.Columns.Add("Changed reservations");
            dataTable.Columns.Add("Renovation suggestions");

            foreach (AccommodationYearStatistic statistic in statistics)
            {
                dataTable.Rows.Add(new object[] { statistic.Year.ToString(), statistic.NumberOfReservations.ToString(), statistic.NumberOfDeclinedReservations.ToString(), statistic.NumberOfChangedReservations.ToString(), statistic.NumberOfRenovationSuggestions.ToString() });
            }

            return dataTable;
        }
    }
}