using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class TourNotificationRepository : ITourNotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/tourNotifications.csv";

        private readonly Serializer<TourNotification> _serializer;

        private List<TourNotification> _notifications;

        public TourNotificationRepository()
        {
            _serializer = new Serializer<TourNotification>();
            _notifications = _serializer.FromCSV(FilePath);
        }
        public List<TourNotification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public ObservableCollection<TourNotification> GetAllByUserId(int userId)
        {
            var notifications = _serializer.FromCSV(FilePath);
            var returnedNotifications = new ObservableCollection<TourNotification>();
            foreach (var notification in notifications)
            {
                if (notification.UserId == userId && notification.Status == NotificationStatus.UNREAD)
                {
                    returnedNotifications.Add(notification);
                }
            }
            return returnedNotifications;
        }

        public int NextId()
        {
            _notifications = _serializer.FromCSV(FilePath);
            if (_notifications.Count < 1)
            {
                return 1;
            }

            return _notifications.Max(c => c.Id) + 1;
        }

        public TourNotification Save(TourNotification tourNotification)
        {
            int id = NextId();
            tourNotification.Id = id;
            _notifications.Add(tourNotification);
            _serializer.ToCSV(FilePath, _notifications);
            return tourNotification;
        }

        public void Update(TourNotification tourNotification)
        {
            _notifications = _serializer.FromCSV(FilePath);
            TourNotification current = _notifications.Find(n => n.Id == tourNotification.Id);
            _notifications.Remove(current);
            _notifications.Add(tourNotification);
            _serializer.ToCSV(FilePath, _notifications);
        }
    }
}
