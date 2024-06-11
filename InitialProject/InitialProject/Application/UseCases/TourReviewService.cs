using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace InitialProject.Application.UseCases
{
    public class TourReviewService
    {
        private readonly ITourReviewRepository _tourReviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICheckpointArrivalRepository _checkpointArrivalRepository;
        private readonly ITourReservationRepository _tourReservationRepository;
        private readonly ICheckpointRepository _checkpointRepository;
        private readonly CheckpointArrivalService _checkpointArrivalService;
        private readonly TourReservationService _tourReservationService;


        public TourReviewService()
        {
            _tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();
            _userRepository = Injector.CreateInstance<IUserRepository>();
            _checkpointArrivalRepository = Injector.CreateInstance<ICheckpointArrivalRepository>();
            _tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            _checkpointRepository = Injector.CreateInstance<ICheckpointRepository>();
            _checkpointArrivalService = new CheckpointArrivalService();
            _tourReservationService = new TourReservationService();
        }

        public IEnumerable<TourReview> GetReviewsByTour(Tour tour)
        {
            List<TourReview> reviews = new();
            foreach(var review in _tourReviewRepository.GetAll())
            {
                var arrival = _checkpointArrivalRepository.GetById(review.ArrivalId);
                var reservation = _tourReservationRepository.GetById(arrival.ReservationId);
                reservation.User = _userRepository.GetById(reservation.UserId);
                arrival.Checkpoint = _checkpointRepository.GetById(arrival.CheckpointId);
                if (reservation.TourId == tour.Id)
                {
                    arrival.Reservation = reservation;
                    review.Arrival = arrival;
                    reviews.Add(review);
                }
            }

            return reviews;
        }

        public void SaveRating(int userId, int tourId, int guideId, int guidesKnowledgeGrade, int guidesLanguageGrade, int interestingGrade, string additionalComment, List<BitmapImage> images)
        {
            var reservation = _tourReservationService.GetByTourIdAndUserId(tourId, userId);
            var arrivals = _checkpointArrivalService.GetAll();
            var arrivalId = -1;
            foreach( var arrival in arrivals)
            {
                if(arrival.ReservationId == reservation.Id)
                {
                    arrivalId = arrival.Id;
                }
            }
            _tourReviewRepository.Save(new TourReview(guidesKnowledgeGrade, guidesLanguageGrade, interestingGrade, additionalComment, arrivalId), images);
        }

        public bool HasAlreadyBeenRated(int tourId, int userId)
        {
            var ratings = GetAll();
            foreach (var rating in ratings)
            {
                if (rating.Arrival.Reservation.TourId == tourId && rating.Arrival.Reservation.UserId == userId)
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<TourReview> GetAll()
        {
            var reviews = _tourReviewRepository.GetAll();
            foreach (var review in reviews)
            {
                review.Arrival = _checkpointArrivalService.GetById(review.ArrivalId);
            }
            return reviews;
        }

        public double? GetAverageGrade(Tour tour)
        {
            var totalGrade = 0.0;
            foreach (var review in this.GetReviewsByTour(tour))
            {
                totalGrade += review.GuideKnowledgeGrade + review.GuideLanguageGrade + review.InterestingnessGrade;
            }
            var numberOfReviews = this.GetReviewsByTour(tour).Count();
            return totalGrade == 0 ? null : totalGrade / numberOfReviews;
        }

        public double? GetAverageGrade(IEnumerable<Tour> tours)
        {
            double? totalGrade = 0.0;
            var tourCounter = 0;
            foreach (var tour in tours)
            {
                var avgGrade = this.GetAverageGrade(tour);
                if (avgGrade is null) continue;
                totalGrade += avgGrade;
                tourCounter++;
            }
            return tourCounter == 0 ? totalGrade : totalGrade / tourCounter;
        }
    }
}
