using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class AccommodationRatingService
    {
        private readonly IAccommodationRatingRepository _accommodationRatingRepository;

        public AccommodationRatingService()
        {
            _accommodationRatingRepository = Injector.CreateInstance<IAccommodationRatingRepository>();
        }

        public AccommodationRating FindAccommodationRatingByReservationId(int reservationId)
        {
            AccommodationRating accommodationRating = _accommodationRatingRepository.FindByReservationId(reservationId);
            return accommodationRating;
        }

        public AccommodationRating SaveAccommodationRating(int cleanliness, int correctness, string comment, int reservationId, int ownerId, int raterId) 
        {
            return _accommodationRatingRepository.Save(cleanliness, correctness, comment, reservationId, ownerId, raterId);
        }

        public bool IsAccommodationRated(int guestId, int reservationId)
        {
            var accommodationRatings = _accommodationRatingRepository.GetAll();
            return accommodationRatings.Exists(x => x.ReservationId == reservationId && x.RaterId == guestId);
        }

        public int CalculateNumberOfRatingsForCorrectness(int i, int ownerId)
        {
            List<AccommodationRating> ownerRatings = _accommodationRatingRepository.GetAll().FindAll(ar => ar.OwnerId == ownerId);

            return ownerRatings.FindAll(or => or.Correctness == i).Count();
        }

        internal int CalculateNumberOfRatingsForCleanliness(int i, int ownerId)
        {
            List<AccommodationRating> ownerRatings = _accommodationRatingRepository.GetAll().FindAll(ar => ar.OwnerId == ownerId);

            return ownerRatings.FindAll(or => or.Cleanliness == i).Count();
        }
    }
}
