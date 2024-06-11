using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class AccommodationRatingImage : ISerializable
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int AccommodationRatingId { get; set; }

        public AccommodationRatingImage() { }

        public AccommodationRatingImage(int id, string url, int accommodationRatingId)
        {
            Id = id;
            Url = url;
            AccommodationRatingId = accommodationRatingId;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Url,
                AccommodationRatingId.ToString()
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Url = values[1];
            AccommodationRatingId = Convert.ToInt32(values[2]);
        }
    }
}
