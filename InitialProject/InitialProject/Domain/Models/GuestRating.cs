using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class GuestRating : ISerializable
    {
        public int Id { get; set; }
        public int Cleanliness { get; set; }
        public int FollowingTheRules { get; set; }
        public string Comment { get; set; }
        public int TheOneWhoIsRatedId { get; set; }
        public int RaterId { get; set; }
        public int ReservationId { get; set; }
        public AccommodationReservation Reservation { get; set; }

        public GuestRating() { }

        public GuestRating(int id, int cleanliness, int followingTheRules, string comment, int theOneWhoIsRatedId, int raterId, int reservationId)
        {
            Id = id;
            Cleanliness = cleanliness;
            FollowingTheRules = followingTheRules;
            Comment = comment;
            TheOneWhoIsRatedId = theOneWhoIsRatedId;
            RaterId = raterId;
            ReservationId = reservationId;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Cleanliness.ToString(),
                FollowingTheRules.ToString(),
                Comment,
                TheOneWhoIsRatedId.ToString(),
                RaterId.ToString(),
                ReservationId.ToString()
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Cleanliness = Convert.ToInt32(values[1]);
            FollowingTheRules = Convert.ToInt32(values[2]);
            Comment = values[3];
            TheOneWhoIsRatedId = Convert.ToInt32(values[4]);
            RaterId = Convert.ToInt32(values[5]);
            ReservationId = Convert.ToInt32(values[6]);
        }
    }
}
