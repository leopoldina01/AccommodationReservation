using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class RequestsOverviewViewModel : ViewModelBase
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

        private string _guestName;
        public string GuestName
        {
            get => _guestName;
            set
            {

                if (value != _guestName)
                {
                    _guestName = value;
                    OnPropertyChanged("GuestName");
                }
            }
        }

        private string _accommodationName;
        public string AccommodationName
        {
            get => _accommodationName;
            set
            {

                if (value != _accommodationName)
                {
                    _accommodationName = value;
                    OnPropertyChanged("AccommodationName");
                }
            }
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged("City");
                }
            }
        }

        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (_country != value)
                {
                    _country = value;
                    OnPropertyChanged("Country");
                }
            }
        }

        private string _type;

        public string Type
        {
            get => _type;
            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        public static ObservableCollection<ReservationRequest> Requests { get; set; }

        private readonly ReservationRequestService _requestService;
        private readonly ManageRequestService _manageRequestService;
        private readonly AccommodationNotificationService _accommodationNotificationService;
        private readonly LocationService _locationService;

        private readonly int _ownerId;

        public List<String> Countries { get; set; }
        public ObservableCollection<String> Cities { get; set; }
        public ObservableCollection<String> Types { get; set; }
        #endregion

        public RequestsOverviewViewModel(int ownerId)
        {
            _requestService = new ReservationRequestService();
            _manageRequestService = new ManageRequestService();
            _accommodationNotificationService = new AccommodationNotificationService();
            _locationService = new LocationService();

            _ownerId = ownerId;

            Requests = new ObservableCollection<ReservationRequest>();
            Countries = new List<String>();
            Cities = new ObservableCollection<String>();
            Types = new ObservableCollection<String>();

            FillInTypes();
            AccommodationRegistrationLoaded();
            LoadOnHoldRequests();
            

            DeclineRequestCommand = new RelayCommand(DeclineRequestCommand_Execute, DeclineRequestCommand_CanExecute);
            AcceptedRequestCommand = new RelayCommand(AcceptedRequestCommand_Execute, AcceptedRequestCommand_CanExecute);
            FilterReservationsCommand = new RelayCommand(FilterReservationsCommand_Execute);
            LoadCitiesCommand = new RelayCommand(LoadCitiesCommand_Execute);
        }

        public void FillInTypes()
        {
            Types.Clear();
            Types.Add("apartment");
            Types.Add("house");
            Types.Add("cottage");
        }

        private void AccommodationRegistrationLoaded()
        {
            Countries = _locationService.GetAllCountries().ToList();
        }

        public void LoadOnHoldRequests()
        {
            Requests.Clear();

            foreach (var request in _requestService.GetOnHoldRequests())
            {
                if (request.Reservation.Accommodation.OwnerId == _ownerId)
                {
                    Requests.Add(request);
                }
            }
        }

        private void FilterRequests()
        {
            Requests.Clear();

            foreach (var request in _requestService.GetRequestsByOwnerId(_ownerId))
            {
                if ((GuestName == null || request.Reservation.Guest.Username.ToUpper().Contains(GuestName.ToUpper())) &&
                    (AccommodationName == null || request.Reservation.Accommodation.Name.ToUpper().Contains(AccommodationName.ToUpper())) &&
                    (Type == null || request.Reservation.Accommodation.Type.ToString().ToUpper().Contains(Type.ToUpper())) &&
                    (City == null || request.Reservation.Accommodation.Location.City.ToString().ToUpper().Contains(City.ToUpper())) &&
                    (Country == null || request.Reservation.Accommodation.Location.Country.ToString().ToUpper().Contains(Country.ToUpper())))
                {
                    Requests.Add(request);
                }

            }
        }

        #region COMMANDS
        public RelayCommand DeclineRequestCommand { get; }
        public RelayCommand AcceptedRequestCommand { get; }
        public RelayCommand FilterReservationsCommand { get; }
        public RelayCommand LoadCitiesCommand { get; }

        public bool DeclineRequestCommand_CanExecute(object? parameter)
        {
            return SelectedRequest is not null;
        }

        public void AcceptedRequestCommand_Execute(object? parameter)
        {
            _manageRequestService.AcceptRequest(SelectedRequest);
            _accommodationNotificationService.NotifyUser($"Date change request for {SelectedRequest.Reservation.Accommodation.Name} is accepted.", _ownerId, SelectedRequest.Reservation.GuestId);
            Requests.Remove(SelectedRequest);
        }

        public bool AcceptedRequestCommand_CanExecute(object? parameter)
        {
            int razlikaUDanima = 0;

            if (SelectedRequest != null)
            {
                razlikaUDanima = (SelectedRequest.Reservation.StartDate - DateTime.Now.Date).Days;
            }
            return SelectedRequest is not null && razlikaUDanima >= SelectedRequest.Reservation.Accommodation.MinDaysBeforeCancel;
        }

        public void DeclineRequestCommand_Execute(object? parameter)
        {
            RequestDeclinedForm requestDeclinedForm = new RequestDeclinedForm(SelectedRequest, _ownerId);
            requestDeclinedForm.Show();
        }

        public void FilterReservationsCommand_Execute(object? parameter)
        {
            FilterRequests();
        }

        public void LoadCitiesCommand_Execute(object? parameter)
        {
            Cities.Clear();
            foreach (var location in _locationService.GetAll())
            {
                if (location.Country != Country) continue;
                Cities.Add(location.City);
            }

            Requests.Clear();

            FilterRequests();
            return;
        }
        #endregion
    }
}
