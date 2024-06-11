using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class RequestedTourNotificationService
    {
        private readonly IRequestedTourNotificationRepository _requestedTourNotificationRepository;
        private readonly TourRequestService _tourRequestService;
        private readonly TourService _tourService;

        public RequestedTourNotificationService()
        {
            _requestedTourNotificationRepository = Injector.CreateInstance<IRequestedTourNotificationRepository>();
            _tourRequestService = new TourRequestService();
            _tourService = new TourService();
        }

        public void Update(RequestedTourNotification notification)
        {
            _requestedTourNotificationRepository.Update(notification);
        }

        public IEnumerable<RequestedTourNotification> GetAllByLocationOrLanguage(User loggedUser)
        {
            List<RequestedTourNotification> notifications = new List<RequestedTourNotification>(); 

            foreach(var notification in _requestedTourNotificationRepository.GetAll())
            {
                foreach(var req in _tourRequestService.GetAll())
                {
                    if((req.Language == _tourService.GetById(notification.TourId).Language || req.LocationId == _tourService.GetById(notification.TourId).LocationId) && req.UserId == loggedUser.Id && req.Status == TourRequestStatus.ON_HOLD)
                    {
                        notifications.Add(notification);
                        break;
                    }
                }
            }
            return notifications;
        }

        public RequestedTourNotification Create(Tour tour)
        {
            var notification = new RequestedTourNotification($"Notification {tour.Id}", NotificationStatus.UNREAD, tour.Id, $"Tour {tour.Name} created!!!");
            return _requestedTourNotificationRepository.Save(notification);
        }

    }
}
