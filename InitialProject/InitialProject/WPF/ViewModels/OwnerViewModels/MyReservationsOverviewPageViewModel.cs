using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.Repositories;
using InitialProject.WPF.Views;
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
    public class MyReservationsOverviewPageViewModel : ViewModelBase
    {
        #region
        private AccommodationReservation _selectedReservation;
        public AccommodationReservation SelectedReservation
        {
            get
            {
                return _selectedReservation;
            }
            set
            {
                if (value != _selectedReservation)
                {
                    _selectedReservation = value;
                    OnPropertyChanged(nameof(SelectedReservation));
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
        private readonly GuestRatingService _ratingService;
        private readonly LocationService _locationService;

        private readonly GuestRatingRepository _ratingRepository;

        private readonly User _owner;

        public List<String> Countries { get; set; }
        public ObservableCollection<String> Cities { get; set; }
        public ObservableCollection<String> Types { get; set; }

        public ObservableCollection<AccommodationReservation> AccommodationReservations { get; set; }
        #endregion

        public MyReservationsOverviewPageViewModel(User user)
        {
            _accommodationReservationService = new AccommodationReservationService();
            _ratingService = new GuestRatingService();

            _ratingRepository = new GuestRatingRepository();
            _locationService = new LocationService();

            _owner = user;


            Countries = new List<String>();
            Cities = new ObservableCollection<String>();
            Types = new ObservableCollection<String>();
            AccommodationReservations = new ObservableCollection<AccommodationReservation>();

            Countries.Clear();

            AccommodationRegistrationLoaded();
            LoadReservations();
            FillInTypes();

            SeeReviewCommand = new RelayCommand(SeeReviewCommand_Execute, SeeReviewCommand_CanExecute);
            ReviewCommand = new RelayCommand(ReviewCommand_Execute, ReviewCommand_CanExecute);
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

        public void LoadReservations()
        {
            AccommodationReservations.Clear();

            foreach (var accommodationReservation in _accommodationReservationService.GetAllByOwnerId(_owner.Id))
            {
                AccommodationReservations.Add(accommodationReservation);
            }
        }

        private void FilterReservations()
        {
            AccommodationReservations.Clear();

            foreach (var reservation in _accommodationReservationService.GetAllByOwnerId(_owner.Id))
            {
                if ((GuestName == null || reservation.Guest.Username.ToUpper().Contains(GuestName.ToUpper())) &&
                    (AccommodationName == null || reservation.Accommodation.Name.ToUpper().Contains(AccommodationName.ToUpper())) &&
                    (Type == null || reservation.Accommodation.Type.ToString().ToUpper().Contains(Type.ToUpper())) &&
                    (City == null || reservation.Accommodation.Location.City.ToString().ToUpper().Contains(City.ToUpper())) &&
                    (Country == null || reservation.Accommodation.Location.Country.ToString().ToUpper().Contains(Country.ToUpper())))
                {
                    AccommodationReservations.Add(reservation);
                }

            }
        }

        #region COMMANDS
        public RelayCommand SeeReviewCommand { get; }
        public RelayCommand ReviewCommand { get; }
        public RelayCommand FilterReservationsCommand { get; }
        public RelayCommand LoadCitiesCommand { get; }

        public bool SeeReviewCommand_CanExecute(object? parameter)
        {
            return SelectedReservation is not null && _ratingService.FindRatingByReservationId(SelectedReservation.Id) != null;
        }

        public void SeeReviewCommand_Execute(object? parameter)
        {
            RatingOverviewWindow ratingOverviewWindow = new RatingOverviewWindow(SelectedReservation);
            ratingOverviewWindow.Show();
        }

        /*public bool SeeReviewCommand_CanExecute(object? parameter)
        {
            AccommodationReservation reservation = parameter as AccommodationReservation;
            return reservation is not null && _ratingService.FindRatingByReservationId(reservation.Id) != null;
        }

        public void SeeReviewCommand_Execute(object? parameter)
        {
            AccommodationReservation reservation = parameter as AccommodationReservation;
            RatingOverviewWindow ratingOverviewWindow = new RatingOverviewWindow(reservation);
            ratingOverviewWindow.Show();
        }*/

        public bool ReviewCommand_CanExecute(object? parameter)
        {
            return SelectedReservation is not null && _ratingService.FindRatingByReservationId(SelectedReservation.Id) == null;
        }

        public void ReviewCommand_Execute(object? parameter)
        {
            if ((DateTime.Now - SelectedReservation.EndDate).Days < 0)
            {
                MessageBox.Show("Selected reservation can't be rated", "Guest hasn't left yet", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else if (SelectedReservation != null && (DateTime.Now - SelectedReservation.EndDate).Days <= 5 && _ratingRepository.GetAll().Find(r => r.ReservationId == SelectedReservation.Id) == null)
            {
                RatingGuestForm ratingGuestForm1 = new RatingGuestForm(_ratingRepository, SelectedReservation, _owner.Id);
                ratingGuestForm1.ShowDialog();
                return;
            }
            else if ((DateTime.Now - SelectedReservation.EndDate).Days > 5)
            {
                MessageBox.Show("Selected reservation can't be rated", "It's been more than 5 days", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else if (_ratingRepository.GetAll().Find(r => r.ReservationId == SelectedReservation.Id) != null)
            {
                MessageBox.Show("Selected reservation can't be rated", "It is already rated", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            RatingGuestForm ratingGuestForm = new RatingGuestForm(_ratingRepository, SelectedReservation, _owner.Id);
            ratingGuestForm.Show();
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

            AccommodationReservations.Clear();

            FilterReservations();
            return;
        }
        #endregion
    }
}
