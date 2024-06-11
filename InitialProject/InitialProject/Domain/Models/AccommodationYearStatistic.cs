using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace InitialProject.Domain.Models
{
    public class AccommodationYearStatistic : ISerializable
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public Accommodation Accommodation { get; set; }
        public int AccommodationId { get; set; }
        public int NumberOfReservations { get; set; }
        public int NumberOfDeclinedReservations { get; set; }
        public int NumberOfChangedReservations { get; set; }
        public int NumberOfRenovationSuggestions { get; set; }
        
        public AccommodationYearStatistic() { }

        public AccommodationYearStatistic(int id, int year, Accommodation accommodation, int accommodationId, int numberOfReservations, int numberOfDeclinedReservations, int numberOfChangedReservations, int numberOfRenovationSuggestions)
        {
            Id = id;
            Year = year;
            Accommodation = accommodation;
            AccommodationId = accommodationId;
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
                Year.ToString(),
                AccommodationId.ToString(),
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
            Year = Convert.ToInt32(values[1]);
            AccommodationId = Convert.ToInt32(values[2]);
            NumberOfReservations = Convert.ToInt32(values[3]);
            NumberOfDeclinedReservations = Convert.ToInt32(values[4]);
            NumberOfChangedReservations= Convert.ToInt32(values[5]);
            NumberOfRenovationSuggestions = Convert.ToInt32(values[6]);
        }
    }
}
