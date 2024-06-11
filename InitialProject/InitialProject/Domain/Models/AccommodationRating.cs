using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;

namespace InitialProject.Domain.Models
{
    public class AccommodationRating : ISerializable
    {
        public int Id { get; set; }
        public int Cleanliness { get; set; }
        public int Correctness { get; set; }
        public string Comment { get; set; }
        public int ReservationId { get; set; }
        public int OwnerId { get; set; }
        public int RaterId { get; set; }

        public AccommodationRating() { }

        public AccommodationRating(int id, int cleanliness, int correctness, string comment, int reservationId, int ownerId, int raterId)
        {
            Id = id;
            Cleanliness = cleanliness;
            Correctness = correctness;
            Comment = comment;
            ReservationId = reservationId;
            OwnerId = ownerId;
            RaterId = raterId;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Cleanliness.ToString(),
                Correctness.ToString(),
                Comment,
                ReservationId.ToString(),
                OwnerId.ToString(),
                RaterId.ToString(),
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Cleanliness = Convert.ToInt32(values[1]);
            Correctness = Convert.ToInt32(values[2]);
            Comment = values[3];
            ReservationId = Convert.ToInt32(values[4]);
            OwnerId = Convert.ToInt32(values[5]);
            RaterId = Convert.ToInt32(values[6]);
        }
    }
}
