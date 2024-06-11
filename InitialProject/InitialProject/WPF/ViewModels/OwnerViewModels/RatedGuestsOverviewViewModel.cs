using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.Repositories;
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
    public class RatedGuestsOverviewViewModel : ViewModelBase
    {
        #region PROPERTIES
        private AccommodationReservation _selectedAccommodationReservation;
        public AccommodationReservation SelectedAccommodationReservation
        {
            get 
            { 
                return _selectedAccommodationReservation;
            }
            set
            {
                if (_selectedAccommodationReservation != value)
                {
                    _selectedAccommodationReservation = value;
                    OnPropertyChanged(nameof(SelectedAccommodationReservation));
                }
            }
        }

        private double _totalRating;
        public double TotalRating
        {
            get
            {
                return _totalRating;
            }
            set
            {
                if (_totalRating != value)
                {
                    _totalRating = value;
                    OnPropertyChanged(nameof(TotalRating));
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

        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly SetOwnerRoleService _setOwnerRoleService;
        private readonly LocationService _locationService;

        private readonly int _ownerId;

        public List<String> Countries { get; set; }
        public ObservableCollection<String> Cities { get; set; }
        public ObservableCollection<String> Types { get; set; }

        public ObservableCollection<AccommodationReservation> RatedReservations { get; set; }
        #endregion

        public RatedGuestsOverviewViewModel(int ownerId)
        {
            _accommodationReservationService = new AccommodationReservationService();
            _setOwnerRoleService = new SetOwnerRoleService();
            _locationService = new LocationService();

            _ownerId = ownerId;

            Countries = new List<String>();
            Cities = new ObservableCollection<String>();
            Types = new ObservableCollection<String>();
            RatedReservations = new ObservableCollection<AccommodationReservation>();

            Countries.Clear();

            FillInTypes();
            AccommodationRegistrationLoaded();

            SeeReviewCommand = new RelayCommand(SeeReviewCommand_Execute, SeeReviewCommand_CanExecute);
            FilterReservationsCommand = new RelayCommand(FilterReservationsCommand_Execute);
            LoadCitiesCommand = new RelayCommand(LoadCitiesCommand_Execute);

            LoadRatedReservations();
            CalculateTotalRating();
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

        public void LoadRatedReservations()
        {
            RatedReservations.Clear();

            foreach (var reservation in _accommodationReservationService.GetRatedReservations(_ownerId))
            {
                RatedReservations.Add(reservation);
            }
        }

        private void FilterReservations()
        {
            RatedReservations.Clear();

            foreach (var reservation in _accommodationReservationService.GetRatedReservations(_ownerId))
            {
                if ((GuestName == null || reservation.Guest.Username.ToUpper().Contains(GuestName.ToUpper())) &&
                    (AccommodationName == null || reservation.Accommodation.Name.ToUpper().Contains(AccommodationName.ToUpper())) &&
                    (Type == null || reservation.Accommodation.Type.ToString().ToUpper().Contains(Type.ToUpper())) &&
                    (City == null || reservation.Accommodation.Location.City.ToString().ToUpper().Contains(City.ToUpper())) &&
                    (Country == null || reservation.Accommodation.Location.Country.ToString().ToUpper().Contains(Country.ToUpper())))
                {
                    RatedReservations.Add(reservation);
                }

            }
        }

        private void CalculateTotalRating()
        {
            TotalRating = Math.Round(_setOwnerRoleService.CalculateTotalRating(_ownerId), 2);
        }

        #region COMMANDS
        public RelayCommand SeeReviewCommand { get; }
        public RelayCommand CloseWindowCommand { get; }
        public RelayCommand FilterReservationsCommand { get; }
        public RelayCommand LoadCitiesCommand { get; }


        public bool SeeReviewCommand_CanExecute(object? parameter)
        {
            return SelectedAccommodationReservation is not null;
        }

        public void SeeReviewCommand_Execute(object? parameter)
        {
            RatingOverviewWindow ratingOverviewWindow = new RatingOverviewWindow(SelectedAccommodationReservation);
            ratingOverviewWindow.Show();
        }

        public void FilterReservationsCommand_Execute(object? parameter)
        {
            FilterReservations();
        }

        public void LoadCitiesCommand_Execute(object? parameter)
        {
            Cities.Clear();
            foreach (var location in _locationService.GetAll())
            {
                if (location.Country != Country) continue;
                Cities.Add(location.City);
            }

            RatedReservations.Clear();

            FilterReservations();
            return;
        }
        #endregion
    }
}
