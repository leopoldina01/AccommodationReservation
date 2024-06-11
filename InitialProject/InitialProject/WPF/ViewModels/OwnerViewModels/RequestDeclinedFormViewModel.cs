using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class RequestDeclinedFormViewModel : ViewModelBase
    {
        #region PROPERTIES
        private ReservationRequest _selectedRequest;
        public ReservationRequest SelectedRequest
        {
            get
            {
                return _selectedRequest;
            }
            set
            {
                if (value != _selectedRequest)
                {
                    _selectedRequest = value;
                    OnPropertyChanged(nameof(SelectedRequest));
                }
            }
        }

        private readonly Window _requestDeclinedForm;
        private readonly ManageRequestService _manageRequestService;
        private readonly AccommodationNotificationService _accommodationNotificationService;

        private int _ownerId;
        #endregion

        public RequestDeclinedFormViewModel(Window requestDeclinedForm, ReservationRequest selectedRequest, int ownerId)
        {
            _requestDeclinedForm = requestDeclinedForm;

            _ownerId = ownerId;

            SelectedRequest = selectedRequest;
            _manageRequestService = new ManageRequestService();
            _accommodationNotificationService = new AccommodationNotificationService();

            CloseWindowCommand = new RelayCommand(CloseWindowCommand_Execute);
            ConfirmDeclineCommand = new RelayCommand(ConfirmDeclineCommand_Execute);
        }

        #region COMMANDS
        public RelayCommand CloseWindowCommand { get; }
        public RelayCommand ConfirmDeclineCommand { get; }

        public void ConfirmDeclineCommand_Execute(object? parameter)
        {
            _manageRequestService.DeclineRequest(SelectedRequest);
            _accommodationNotificationService.NotifyUser($"Date change request for {SelectedRequest.Reservation.Accommodation.Name} is declined.", _ownerId, SelectedRequest.Reservation.GuestId);
            RequestsOverviewViewModel.Requests.Remove(SelectedRequest);
            _requestDeclinedForm.Close();
        }
        public void CloseWindowCommand_Execute(object? parameter)
        {
            _requestDeclinedForm.Close();
        }
        #endregion
    }
}
