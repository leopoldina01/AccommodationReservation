using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class RequestedTourNotificationViewModel : ViewModelBase
    {
        #region PROPERTIES
        public User LoggedUser { get; set; }

        private RequestedTourNotification _selectedNotification;
        public RequestedTourNotification SelectedNotification
        {
            get
            {
                return _selectedNotification;
            }
            set
            {
                if (_selectedNotification != value)
                {
                    _selectedNotification = value;
                    OnPropertyChanged(nameof(SelectedNotification));
                }
            }
        }

        public ObservableCollection<RequestedTourNotification> RequestedTourNotifications { get; set; }
        private readonly TourService _tourService;
        private readonly RequestedTourNotificationService _requestedTourNotificationService;

        #endregion

        public RequestedTourNotificationViewModel(User loggedUser)
        {
            LoggedUser = loggedUser;
            RequestedTourNotifications = new ObservableCollection<RequestedTourNotification>();
            _tourService = new TourService();
            _requestedTourNotificationService = new RequestedTourNotificationService();

            ViewNotificationCommand = new RelayCommand(ViewNotificationCommand_Execute);
            ShowTourNotificationsCommand = new RelayCommand(ShowTourNotificationsCommand_Execute);

            LoadNotifications();
        }

        public void LoadNotifications()
        {
            foreach(var notification in _requestedTourNotificationService.GetAllByLocationOrLanguage(LoggedUser))
            {
                RequestedTourNotifications.Add(notification);
            }
        }

        #region COMMANDS

        public RelayCommand ViewNotificationCommand { get; }
        public RelayCommand ShowTourNotificationsCommand { get; }

        public void ShowTourNotificationsCommand_Execute(object? parameter)
        {
            TourNotificationsView tourNotificationsView = new TourNotificationsView(LoggedUser);
        }

        public void ViewNotificationCommand_Execute(object? parameter)
        {
            if (SelectedNotification != null)
            {
                SelectedNotification.Status = NotificationStatus.READ;
                _requestedTourNotificationService.Update(SelectedNotification);
                Tour tour = _tourService.GetById(SelectedNotification.TourId);
                SelectedTourView selectedTourView = new SelectedTourView(tour, LoggedUser);
                //selectedTourView.Show();
                //_requestedTourNotificationsView.Close();
            }
        }

        #endregion
    }
}
