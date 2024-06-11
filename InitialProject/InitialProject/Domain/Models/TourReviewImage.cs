using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class TourReviewImage : ISerializable
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int ReviewId { get; set; }

        public TourReviewImage() { }

        public TourReviewImage(string url, int reviewId)
        {
            Url = url;
            ReviewId = reviewId;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Url,
                ReviewId.ToString()
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Url = values[1];
            ReviewId = Convert.ToInt32(values[2]);
        }
    }
}
