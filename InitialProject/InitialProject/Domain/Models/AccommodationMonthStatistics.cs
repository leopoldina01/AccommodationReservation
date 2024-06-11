using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class AccommodationMonthStatistics : ISerializable
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public AccommodationYearStatistic YearStatistics { get; set; }
        public int YearStatisticsId { get; set; }
        public int NumberOfReservations { get; set; }
        public int NumberOfDeclinedReservations { get; set; }
        public int NumberOfChangedReservations { get; set; }
        public int NumberOfRenovationSuggestions { get; set; }

        public AccommodationMonthStatistics() { }

        public AccommodationMonthStatistics(int id, int month, AccommodationYearStatistic yearStatistics, int yearStatisticsId, int numberOfReservations, int numberOfDeclinedReservations, int numberOfChangedReservations, int numberOfRenovationSuggestions)
        {
            Id = id;
            Month = month;
            YearStatistics = yearStatistics;
            YearStatisticsId = yearStatisticsId;
            NumberOfReservations = numberOfReservations;
            NumberOfDeclinedReservations = numberOfDeclinedReservations;
            NumberOfChangedReservations = numberOfChangedReservations;
            NumberOfRenovationSuggestions = numberOfRenovationSuggestions;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Month.ToString(),
                YearStatisticsId.ToString(),
                NumberOfReservations.ToString(),
                NumberOfDeclinedReservations.ToString(),
                NumberOfChangedReservations.ToString(),
                NumberOfRenovationSuggestions.ToString()
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Month = Convert.ToInt32(values[1]);
            YearStatisticsId = Convert.ToInt32(values[2]);
            NumberOfReservations = Convert.ToInt32(values[3]);
            NumberOfDeclinedReservations = Convert.ToInt32(values[4]);
            NumberOfChangedReservations = Convert.ToInt32(values[5]);
            NumberOfRenovationSuggestions = Convert.ToInt32(values[6]);
        }
    }
}
