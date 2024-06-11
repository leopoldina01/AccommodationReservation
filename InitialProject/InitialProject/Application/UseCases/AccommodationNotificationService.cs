using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Models;

namespace InitialProject.Application.UseCases
{
    public class AccommodationNotificationService
    {
        private readonly IAccommodationNotificationRepository _accommodationNotificationRepository;
        private readonly AccommodationService _accommodationService;
        public AccommodationNotificationService()
        {
            _accommodationNotificationRepository = Injector.CreateInstance<IAccommodationNotificationRepository>();
            _accommodationService = new AccommodationService();
        }

        public List<AccommodationNotification> GetAllByReceiverId(int receiverId)
        {
            var receiversNotifications = new List<AccommodationNotification>();
            var notifications = _accommodationNotificationRepository.GetAll();
            foreach(var notification in notifications)
            {
                if(notification.ReceiverId == receiverId)
                {
                    receiversNotifications.Add(notification);
                }
            }

            return receiversNotifications;
        }

        public List<AccommodationNotification> GetUnseenNotifications(List<AccommodationNotification> notifications)
        {
            var unseenNotifications = new List<AccommodationNotification>();
            foreach (var notification in notifications)
            {
                if (!notification.Seen)
                {
                    unseenNotifications.Add(notification);
                }
            }

            return unseenNotifications;
        }

        public void SetAsSeen(AccommodationNotification notification)
        {
            notification.Seen = true;
            _accommodationNotificationRepository.Update(notification);
        }

        public void NotifyUser(string textContent, int senderId, int receiverId)
        {
            var newNotification = new AccommodationNotification(textContent, false, senderId, receiverId);
            _accommodationNotificationRepository.Save(newNotification);
        }

        public void NotifyAllOwners(int locationId, int guestId, string guestUsername, Location location)
        {
            List<int> ownersId = new List<int>();

            List<Accommodation> accommodations = _accommodationService.GetAll().FindAll(a => a.LocationId == locationId);

            foreach(var accommodation in accommodations)
            {
                if (!ownersId.Contains(accommodation.OwnerId))
                {
                    ownersId.Add(accommodation.OwnerId);
                }
            }

            foreach(var ownerId in ownersId)
            {
                NotifyUser($"{guestUsername} has created forum for {location.City}  {location.Country}.", guestId, ownerId);
            }
        }
    }
}
