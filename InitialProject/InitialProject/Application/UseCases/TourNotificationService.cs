using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class TourNotificationService
    {
        private readonly ITourNotificationRepository _tourNotificationRepository;
        public TourNotificationService()
        {
            _tourNotificationRepository = Injector.CreateInstance<ITourNotificationRepository>();
        }

        public ObservableCollection<TourNotification> GetNotificationsByUser(int userId)
        {
            return _tourNotificationRepository.GetAllByUserId(userId);
        }

        public void UpdateNotification(TourNotification notification)
        {
            _tourNotificationRepository.Update(notification);
        }

        public TourNotification Create(CheckpointArrival arrival)
        {
            var text = $"You have been added to tour '{arrival.Reservation.Tour.Name}' at checkpoint '{arrival.Checkpoint.Name}'";
            var notification = new TourNotification(text, NotificationStatus.UNREAD, DateTime.Now, arrival.Reservation.UserId, arrival.Id, "Notification");
            return _tourNotificationRepository.Save(notification);
        }
    }
}
