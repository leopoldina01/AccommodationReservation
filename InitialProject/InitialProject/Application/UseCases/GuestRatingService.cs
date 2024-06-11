using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class GuestRatingService
    {
        private readonly IGuestRatingRepository _ratingRepository;
        private readonly AccommodationRatingService _accommodationRatingService;
        private readonly AccommodationReservationService _accommodationReservationService;
        public GuestRatingService() 
        { 
            _ratingRepository = Injector.CreateInstance<IGuestRatingRepository>();
            _accommodationRatingService = new AccommodationRatingService();
            _accommodationReservationService = new AccommodationReservationService();
        }

        public GuestRating FindRatingByReservationId(int reservationId)
        {
            GuestRating rating = _ratingRepository.GetByReservationId(reservationId);

            return rating;
        }

        public List<GuestRating> FindAllRatingsByGuestId(int guestId)
        {
            var guestRatings = _ratingRepository.GetAll();
            List<GuestRating> newRatings = new List<GuestRating>();
            foreach (var rating in guestRatings)
            {
                if (rating.TheOneWhoIsRatedId == guestId)
                    newRatings.Add(rating);
            }
            
            return newRatings;
        }

        public List<GuestRating> RemoveNonmutualRatings(List<GuestRating> guestRatings)
        {
            var newRatings = new List<GuestRating>();
            foreach (var rating in guestRatings)
            {
                if (_accommodationRatingService.IsAccommodationRated(rating.TheOneWhoIsRatedId, rating.ReservationId))
                    newRatings.Add(rating);
            }
            return newRatings;
        }

        public void LoadReservations(List<GuestRating> guestRatings)
        {
            foreach (var rating in guestRatings)
            {
                rating.Reservation = _accommodationReservationService.GetById(rating.ReservationId);
                _accommodationReservationService.LoadAccommodation(rating.Reservation);
            }
        }

        public List<GuestRating> GetAll()
        {
            return _ratingRepository.GetAll();
        }
    }
}
