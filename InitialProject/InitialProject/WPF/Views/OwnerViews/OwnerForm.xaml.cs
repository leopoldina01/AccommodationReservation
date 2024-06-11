using InitialProject.Application.UseCases;
using InitialProject.Domain.Models;
using InitialProject.Repositories;
using InitialProject.WPF.Views.Guest1Views;
using InitialProject.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerForm.xaml
    /// </summary>
    public partial class OwnerForm : Window
    {
        private readonly AccommodationRepository _accommodationRepository;
        private readonly LocationRepository _locationRepository;
        private readonly AccommodationImageRepository _imageRepository;
        private readonly AccommodationReservationRepository _reservationRepository;
        private readonly UserRepository _userRepository;
        private readonly GuestRatingRepository _ratingRepository;
        private readonly AccommodationNotificationService _accommodationNotificationService;

        public int _numberOfUnratedGuests;

        private int _ownerId;
        public OwnerForm(AccommodationRepository accommodationRepository, LocationRepository locationRepository, AccommodationImageRepository imageRepository, User user, AccommodationReservationRepository reservationRepository, UserRepository userRepository, GuestRatingRepository ratingRepository)
        {
            InitializeComponent();
            this.DataContext = this;
            _accommodationRepository = accommodationRepository;
            _locationRepository = locationRepository;
            _imageRepository = imageRepository;
            LabelWelcomeUser.Content = "Welcome " + user.Username;
            _ownerId = user.Id;
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _ratingRepository = ratingRepository;
            _accommodationNotificationService = new AccommodationNotificationService();

            _numberOfUnratedGuests = 0;

            //OpenReminderWindow();
        }

        public void OpenReminderWindow()
        {
            foreach (var reservation in _reservationRepository.GetAll())
            {
                TimeSpan time = DateTime.Now - reservation.EndDate;

                if (time.Days > 5 || DateTime.Now < reservation.EndDate)
                {
                    continue;
                }

                int reservationId = reservation.Id;
                Accommodation foundAccommodation = _accommodationRepository.GetAll().Find(a => a.Id == reservation.AccommodationId);

                if (foundAccommodation != null)
                {
                    GuestRating foundRating = FindRating(reservation);

                    if (foundAccommodation.OwnerId == _ownerId && foundRating.Id == 0)
                    {/*
                        RatingGuestReminderForm ratingGuestReminderForm = new RatingGuestReminderForm(_ownerId, _reservationRepository, _accommodationRepository, _userRepository, _ratingRepository);
                        ratingGuestReminderForm.ShowDialog();*/
                        break;
                    }
                }
            }
        }

        public void OpenNotificationWindow()
        {
            var notifications = _accommodationNotificationService.GetAllByReceiverId(_ownerId);
            notifications = _accommodationNotificationService.GetUnseenNotifications(notifications);

            if (notifications.Count > 0)
            {
                foreach (var notification in notifications)
                {
                    NotificationWindow notificationWindow = new NotificationWindow(notification);
                    notificationWindow.Show();

                    _accommodationNotificationService.SetAsSeen(notification);
                }
            }
        }

        public GuestRating FindRating(AccommodationReservation reservation)
        {
            List<GuestRating> ratings = _ratingRepository.GetAll();
            GuestRating foundRating = new GuestRating();

            foreach (var rating in ratings)
            {
                if (rating.ReservationId == reservation.Id)
                {
                    foundRating = rating;
                }
            }

            return foundRating;
        }

        private void ButtonRegistrateAccommodation_Click(object sender, RoutedEventArgs e)
        {
            /*AccommodationRegistrationForm accommodationRegistration = new AccommodationRegistrationForm(_accommodationRepository, _locationRepository, _imageRepository, _ownerId, _userRepository);
            accommodationRegistration.Show();*/
        }

        private void ButtonSignOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signIn = new SignInForm();
            signIn.Show();
            this.Close();
        }

        private void ButtonRateGuest_Click(object sender, RoutedEventArgs e)
        {
            GuestsOverview guestsOverview = new GuestsOverview(_ownerId, _reservationRepository, _accommodationRepository, _userRepository, _ratingRepository);
            guestsOverview.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenReminderWindow();
            OpenNotificationWindow();
        }

        private void ButtonProfile_Click(object sender, RoutedEventArgs e)
        {
            OwnerProfileOverview ownerProfileOverview = new OwnerProfileOverview(_ownerId);
            ownerProfileOverview.Show();
        }
    }
}
