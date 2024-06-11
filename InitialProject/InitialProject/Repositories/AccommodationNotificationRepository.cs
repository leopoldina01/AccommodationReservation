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
    public class AccommodationNotificationRepository : IAccommodationNotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationNotifications.csv";

        private readonly Serializer<AccommodationNotification> _serializer;

        private List<AccommodationNotification> _notifications;

        public AccommodationNotificationRepository()
        {
            _serializer = new Serializer<AccommodationNotification>();
            _notifications = _serializer.FromCSV(FilePath);
        }
        public List<AccommodationNotification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
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

        public AccommodationNotification Save(AccommodationNotification accommodationNotification)
        {
            int id = NextId();
            accommodationNotification.Id = id;
            _notifications.Add(accommodationNotification);
            _serializer.ToCSV(FilePath, _notifications);
            return accommodationNotification;
        }

        public void Update(AccommodationNotification updatedNotification)
        {
            _notifications = _serializer.FromCSV(FilePath);
            foreach(var notification in _notifications)
            {
                if(notification.Id == updatedNotification.Id)
                {
                    notification.TextContent = updatedNotification.TextContent;
                    notification.Seen = updatedNotification.Seen;
                    notification.SenderId = updatedNotification.SenderId;
                    notification.ReceiverId = updatedNotification.ReceiverId;
                }
            }
            _serializer.ToCSV(FilePath, _notifications);
        }
    }
}
