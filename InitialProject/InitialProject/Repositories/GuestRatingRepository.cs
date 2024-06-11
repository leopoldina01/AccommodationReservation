using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class GuestRatingRepository : IGuestRatingRepository
    {
        private const string FilePath = "../../../Resources/Data/guestRatings.csv";

        private readonly Serializer<GuestRating> _serializer;

        private List<GuestRating> _ratings;

        public GuestRatingRepository()
        {
            _serializer = new Serializer<GuestRating>();
            _ratings = _serializer.FromCSV(FilePath);
        }

        public List<GuestRating> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public GuestRating Save(string cleanliness, string followingTheRules, string comment, int theOneWhoIsRatedId, int raterId, int reservationId)
        {
            int id = NextId();

            GuestRating rating = new GuestRating(id, Convert.ToInt32(cleanliness), Convert.ToInt32(followingTheRules), comment, theOneWhoIsRatedId, raterId, reservationId);

            _ratings = _serializer.FromCSV(FilePath);
            _ratings.Add(rating);
            _serializer.ToCSV(FilePath, _ratings);
            return rating;
        }

        public int NextId()
        {
            _ratings = _serializer.FromCSV(FilePath);

            if (_ratings.Count < 1)
            {
                return 1;
            }

            return _ratings.Max(t => t.Id) + 1;
        }

        public GuestRating GetByReservationId(int reservationId)
        {
            _ratings = _serializer.FromCSV(FilePath);

            GuestRating rating = _ratings.Find(r => r.ReservationId == reservationId);

            return rating;
        }
    }
}
