using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;

namespace InitialProject
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
            { typeof(IUserRepository), new UserRepository() },
            { typeof(ITourRepository), new TourRepository() },
            { typeof(ILocationRepository), new LocationRepository() },
            { typeof(IAccommodationReservationRepository), new AccommodationReservationRepository() },
            { typeof(IGuestRatingRepository), new GuestRatingRepository() },
            { typeof(IAccommodationRepository), new AccommodationRepository() },
            { typeof(IAccommodationRatingRepository), new AccommodationRatingRepository() },
            { typeof(ICheckpointArrivalRepository), new CheckpointArrivalRepository() },
            { typeof(ITourReviewRepository), new TourReviewRepository() },
            { typeof(ITourReservationRepository), new TourReservationRepository() },
            { typeof(ICheckpointRepository), new CheckpointRepository() },
            { typeof(IReservationRequestRepository), new ReservationRequestRepository() },
            { typeof(IAccommodationRatingImageRepository), new AccommodationRatingImageRepository() },
            { typeof(ITourReviewImageRepository), new TourReviewImageRepository() },
            { typeof(ITourNotificationRepository), new TourNotificationRepository() },
            { typeof(ITourImageRepository), new TourImageRepository() },
            { typeof(IAccommodationNotificationRepository), new AccommodationNotificationRepository() },
            { typeof(IVoucherRepository), new VoucherRepository() },
            { typeof(IAccommodationImageRepository), new AccommodationImageRepository() },
            { typeof(ITourRequestRepository), new TourRequestRepository() },
            { typeof(IAccommodationRenovationRepository), new AccommodationRenovationRepository() },
            { typeof(IAccommodationYearStatisticsRepository), new AccommodationYearStatisticsRepository() },
            { typeof(IAccommodationMonthStatisticsRepository), new AccommodationMonthStatisticsRepository() },
            { typeof(IAccommodationRenovationSuggestionRepository), new AccommodationRenovationSuggestionRepository() },
            { typeof(IRequestedTourNotificationRepository), new RequestedTourNotificationRepository() },
            { typeof(ISuperGuestRepository), new SuperGuestRepository() },
            { typeof(IForumRepository), new ForumRepository() },
            { typeof(ICommentRepository), new CommentRepository() },
            { typeof(IReportedCommentRepository), new ReportedCommentRepository() },
            { typeof(IComplexTourRequestRepository), new ComplexTourRequestRepository() },
            { typeof(IComplexTourPartRepository), new ComplexTourPartRepository() },
            { typeof(ISuperGuideRepository), new SuperGuideRepository() }
        };

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}
