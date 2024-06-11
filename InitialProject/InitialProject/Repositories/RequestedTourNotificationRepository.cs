using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class RequestedTourNotificationRepository : IRequestedTourNotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/requestedTourNotifications.csv";

        private readonly Serializer<RequestedTourNotification> _serializer;

        private List<RequestedTourNotification> _notifications;

        public RequestedTourNotificationRepository()
        {
            _serializer = new Serializer<RequestedTourNotification>();
            _notifications = _serializer.FromCSV(FilePath);
        }
        public IEnumerable<RequestedTourNotification> GetAll()
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

        public RequestedTourNotification Save(RequestedTourNotification tourNotification)
        {
            int id = NextId();
            tourNotification.Id = id;
            _notifications.Add(tourNotification);
            _serializer.ToCSV(FilePath, _notifications);
            return tourNotification;
        }

        public void Update(RequestedTourNotification tourNotification)
        {
            _notifications = _serializer.FromCSV(FilePath);
            RequestedTourNotification current = _notifications.Find(n => n.Id == tourNotification.Id);
            _notifications.Remove(current);
            _notifications.Add(tourNotification);
            _serializer.ToCSV(FilePath, _notifications);
        }
    }
}
