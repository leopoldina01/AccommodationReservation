using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class NotificationsViewModel : ViewModelBase
    {
        #region PROPERTIES
        public ObservableCollection<AccommodationNotification> Notifications { get; set; }
        public User LoggedUser { get; set; }

        private readonly AccommodationNotificationService _accommodationNotificationService;
        private readonly UserService _userService;
        #endregion
        public NotificationsViewModel(User user)
        {
            _accommodationNotificationService = new AccommodationNotificationService();
            _userService = new UserService();

            LoggedUser = user;

            Notifications = new ObservableCollection<AccommodationNotification>();

            LoadNotifications();
            MarkAllAsSeen();
        }

        public void LoadNotifications()
        {
            Notifications.Clear();
            var notifications = _accommodationNotificationService.GetAllByReceiverId(LoggedUser.Id);
            foreach(var notification in notifications)
            {
                notification.Sender = _userService.GetById(notification.SenderId);
                if (!notification.Seen)
                {
                    notification.NotificationInfo = "New notification";
                }
                Notifications.Add(notification);
            }
        }

        public void MarkAllAsSeen()
        {
            foreach (var notification in _accommodationNotificationService.GetAllByReceiverId(LoggedUser.Id))
            {
                _accommodationNotificationService.SetAsSeen(notification);
            }
        }
    }
}
