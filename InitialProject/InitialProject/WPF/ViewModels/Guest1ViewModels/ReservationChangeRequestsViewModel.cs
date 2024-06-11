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

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class ReservationChangeRequestsViewModel : ViewModelBase
    {
        #region PROPERTIES
        public ObservableCollection<ReservationRequest> Requests { get; set; }

        private readonly int _guestId;
        private readonly Window _reservationChangeRequestView;
        private readonly ReservationRequestService _requestService;
        #endregion

        public ReservationChangeRequestsViewModel(Window reservationChangeRequestsView, int guestId)
        {
            _reservationChangeRequestView = reservationChangeRequestsView;
            _requestService = new ReservationRequestService();
            _guestId = guestId;
            Requests = new ObservableCollection<ReservationRequest>();

            CloseCommand = new RelayCommand(CloseCommand_Execute);

            LoadRequests();
        }

        private void LoadRequests()
        {
            var requests = _requestService.GetRequestsByGuestId(_guestId);
            foreach (var request in requests)
            {
                Requests.Add(request);
            }
        }

        #region COMMANDS
        public RelayCommand CloseCommand { get; }

        public void CloseCommand_Execute(object? parameter)
        {
            _reservationChangeRequestView.Close();
        }
        #endregion
    }
}
