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
    public class SelectedTourNotificationViewModel : ViewModelBase
    {
        #region PROPERTIES

        public User LoggedUser { get; set; }

        private TourNotification _selectedNotification;
        public TourNotification SelectedNotification
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

        private readonly TourNotificationService _tourNotificationService;
        private readonly CheckpointArrivalService _checkpointArrivalService;

        #endregion
        public SelectedTourNotificationViewModel(User user, TourNotification selectedNotification)
        {
            _tourNotificationService = new TourNotificationService();
            _checkpointArrivalService = new CheckpointArrivalService();
            LoggedUser = user;
            SelectedNotification = selectedNotification;

            ConfirmNotificationCommand = new RelayCommand(ConfirmNotification_Execute);
            CancelNotificationCommand = new RelayCommand(CancelNotificationCommand_Execute);
        }

        #region  COMMANDS

        public RelayCommand ConfirmNotificationCommand { get; }
        public RelayCommand CancelNotificationCommand { get; }

        public void CancelNotificationCommand_Execute(object? parameter)
        {
            SelectedNotification.Status = NotificationStatus.READ;
            _checkpointArrivalService.Remove(SelectedNotification.CheckpointArrivalId);
            _tourNotificationService.UpdateNotification(SelectedNotification);
            TourNotificationsView tourNotificationsView = new TourNotificationsView(LoggedUser);
            //tourNotificationsView.Show();
            //_selectedTourNotificationView.Close();
        }

        public void ConfirmNotification_Execute(object? parameter)
        {
            SelectedNotification.Status = NotificationStatus.READ;
            _tourNotificationService.UpdateNotification(SelectedNotification);
            Guest2TourView guest2TourView = new Guest2TourView(LoggedUser);
            //_selectedTourNotificationView.Close();
        }

        #endregion

    }
}
