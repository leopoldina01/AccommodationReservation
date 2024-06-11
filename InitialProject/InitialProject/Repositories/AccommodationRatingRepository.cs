using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain.Models;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class AccommodationRatingRepository : IAccommodationRatingRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationRatings.csv";

        private readonly Serializer<AccommodationRating> _serializer;

        private List<AccommodationRating> _accommodationRatings;

        public AccommodationRatingRepository()
        {
            _serializer = new Serializer<AccommodationRating>();
            _accommodationRatings = _serializer.FromCSV(FilePath);
        }

        public List<AccommodationRating> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AccommodationRating Save(int cleanliness, int correctness, string comment, int reservationId, int ownerId, int raterId)
        {
            int id = NextId();

            AccommodationRating accommodationRating = new AccommodationRating(id, cleanliness, correctness, comment, reservationId, ownerId, raterId);

            _accommodationRatings.Add(accommodationRating);
            _serializer.ToCSV(FilePath, _accommodationRatings);
            return accommodationRating;
        }

        public int NextId()
        {
            _accommodationRatings = _serializer.FromCSV(FilePath);
            if (_accommodationRatings.Count < 1)
            {
                return 1;
            }

            return _accommodationRatings.Max(c => c.Id) + 1;
        }

        public AccommodationRating FindByReservationId(int reservationId)
        {
            _accommodationRatings = _serializer.FromCSV(FilePath);

            AccommodationRating accommodationRating = _accommodationRatings.Find(ar => ar.ReservationId == reservationId);

            return accommodationRating;
        }
    }
}
