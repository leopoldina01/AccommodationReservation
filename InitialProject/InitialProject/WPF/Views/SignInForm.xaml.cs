using InitialProject.Application.UseCases;
using InitialProject.Domain.Models;
using InitialProject.Repositories;
using InitialProject.WPF.Views.Guest1Views;
using InitialProject.WPF.Views.Guest2Views;
using InitialProject.WPF.Views.OwnerViews;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Windows;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _userRepository;
        private readonly AccommodationRepository _accommodationRepository;
        private readonly LocationRepository _locationRepository;
        private readonly AccommodationImageRepository _accommodationImageRepository;
        private readonly TourRepository _tourRepository;
        private readonly TourImageRepository _tourImageRepository;
        private readonly CheckpointRepository _checkpointRepository;
        private readonly AccommodationReservationRepository _accommodationReservationRepository;
        private readonly GuestRatingRepository _ratingRepository;
        private readonly TourReservationRepository _tourReservationRepository;
        private readonly CheckpointArrivalRepository _checkpointArrivalRepository;
        private readonly AccommodationRatingRepository _accommodationRatingRepository;
        private readonly AccommodationRatingImageRepository _accommodationRatingImageRepository;
        private readonly AccommodationRenovationService _accommodationRenovationService;

        private readonly SetOwnerRoleService _setOwnerRoleService;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _userRepository = new UserRepository();
            _accommodationRepository = new AccommodationRepository();
            _locationRepository = new LocationRepository(); 
            _accommodationImageRepository = new AccommodationImageRepository();
            _tourRepository = new TourRepository();
            _tourImageRepository = new TourImageRepository();
            _checkpointRepository = new CheckpointRepository();
            _accommodationReservationRepository = new AccommodationReservationRepository();
            _ratingRepository = new GuestRatingRepository();
            _tourReservationRepository = new TourReservationRepository();
            _checkpointArrivalRepository = new CheckpointArrivalRepository();
            _accommodationRatingRepository = new AccommodationRatingRepository();
            _accommodationRatingImageRepository = new AccommodationRatingImageRepository();
            _accommodationRenovationService = new AccommodationRenovationService();

            _setOwnerRoleService = new SetOwnerRoleService();

            SetOwnerRole();
            UpdateRenovationInformations();
        }

        private async void UpdateRenovationInformations()
        {
            if (_accommodationRenovationService.GetAllFinishedTodayAndNotMarked().Count() != 0)
            {
                SetAccommodationStatusRenovated(_accommodationRenovationService.GetAllFinishedTodayAndNotMarked());
            }

            if (_accommodationRenovationService.GetAllFinishedInLastYearAndNotMarked().Count() != 0)
            {
                SetAccommodationStatusRenovated(_accommodationRenovationService.GetAllFinishedInLastYearAndNotMarked());
            }

            if (_accommodationRenovationService.GetAllRenovatedBeforeMoreThanAYear().Count() != 0)
            {
                RemoveAccommodationStatusRenovated(_accommodationRenovationService.GetAllRenovatedBeforeMoreThanAYear());
            }

            await Task.Delay(TimeSpan.FromDays(1));
            UpdateRenovationInformations();
        }

        public void RemoveAccommodationStatusRenovated(List<AccommodationRenovation> renovations)
        {
            List<AccommodationRenovation> accommodationRenovations = renovations;

            foreach (var renovation in accommodationRenovations)
            {
                renovation.Accommodation.RenovationStatus = "";
                _accommodationRepository.Update(renovation.Accommodation);
            }
        }

        public void SetAccommodationStatusRenovated(List<AccommodationRenovation> finishedAndNotMarkedRenovations)
        {
            List<AccommodationRenovation> finishedRenovations = finishedAndNotMarkedRenovations;

            foreach (var renovation in finishedRenovations)
            {
                renovation.Accommodation.RenovationStatus = " RENOVATED";
                _accommodationRepository.Update(renovation.Accommodation);
            }

        }

        private void SetOwnerRole()
        {
            foreach (var user in _userRepository.GetAll())
            {
                if (user.Role == UserRole.OWNER || user.Role == UserRole.SUPER_OWNER)
                {
                    _setOwnerRoleService.SetOwnerRole(user.Id);
                }
                
            }
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _userRepository.GetByUsername(Username);

            if (user == null)
            {
                MessageBox.Show("Wrong username!");
                return;
            }

            if (!user.Password.Equals(txtPassword.Password))
            {
                MessageBox.Show("Wrong password!");
                return;
            }

            OpenSuitableWindow(user);
        }

        private void OpenSuitableWindow(User user)
        {
            if (user.Role == UserRole.OWNER || user.Role == UserRole.SUPER_OWNER)
            {
                OwnerMainWindow ownerMainWindow = new OwnerMainWindow(user);
                ownerMainWindow.Show();
                Close();
            }
            else if (user.Role == UserRole.GUEST1)
            {
                //Guest1Menu guest1Menu = new Guest1Menu(_accommodationRepository, _accommodationImageRepository, _locationRepository, _accommodationRatingRepository, _accommodationRatingImageRepository, _accommodationReservationRepository, _userRepository, user);
                //guest1Menu.Show();
                MainWindow guest1MainWindow = new MainWindow(user, _accommodationRepository, _locationRepository, _accommodationImageRepository, _accommodationReservationRepository, _userRepository);
                guest1MainWindow.Show();
                Close();
            }
            else if (user.Role == UserRole.GUEST2)
            {
                //Guest2Menu guest2Menu = new Guest2Menu(user);
                //guest2Menu.Show();
                //Guest2TourView guest2TourView = new Guest2TourView(user);
                //guest2TourView.Show();
                Guest2MainWindow guest2MainWindow = new Guest2MainWindow(user);
                guest2MainWindow.Show();
                Close();
            }
            else if (user.Role == UserRole.GUIDE)
            {
                GuideMenuView guideMenu = new GuideMenuView(user);
                guideMenu.Show();
                Close();
            }
        }
    }
}
