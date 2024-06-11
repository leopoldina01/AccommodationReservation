using InitialProject.Commands;
using InitialProject.WPF.Views.OwnerViews;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InitialProject.WPF.Views;
using System.Windows.Controls;
using InitialProject.Repositories;
using InitialProject.Application.UseCases;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.WPF.Views.Guest1Views;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class OwnerMainWindowViewModel : ViewModelBase
    {
        #region PROPERTIES
        private readonly User _user;
        private readonly Window _ownerMainWindow;

        public String Username { get; set; }

        private Page _selectedPage;
        public Page SelectedPage 
        {
            get
            {
                return _selectedPage;
            }
            set
            {
                if (_selectedPage != value)
                {
                    _selectedPage = value;
                    OnPropertyChanged(nameof(SelectedPage));
                }
            }
        }

        private readonly AccommodationRenovationService _accommodationRenovationService;
        private readonly AccommodationService _accommodationService;
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly GuestRatingService _guestRatingService;
        private readonly AccommodationNotificationService _accommodationNotificationService;
        #endregion

        public OwnerMainWindowViewModel(Window ownerMainWindow, User user) 
        {
            _accommodationRenovationService = new AccommodationRenovationService();
            _accommodationService = new AccommodationService();
            _accommodationReservationService = new AccommodationReservationService();
            _guestRatingService = new GuestRatingService();
            _accommodationNotificationService = new AccommodationNotificationService();

            _ownerMainWindow = ownerMainWindow;
            _user = user;
            SelectedPage = new MyAccommodationsPage(_user);

            Username = user.Username;

            CloseWindowCommand = new RelayCommand(CloseWindowCommand_Execute);
            SeeMyAccommodationsCommand = new RelayCommand(SeeMyAccommodationsCommand_Execute, SeeMyAccommodationsCommand_CanExecute);
            SeeMyReservationsCommand = new RelayCommand(SeeMyReservationsCommand_Execute, SeeMyReservationsCommand_CanExecute);
            SeeMyRequestsCommand = new RelayCommand(SeeMyRequestsCommand_Execute, SeeMyRequestsCommand_CanExecute);
            SeeMyReviewsCommand = new RelayCommand(SeeMyReviewsCommand_Execute, SeeMyReviewsCommand_CanExecute);
            SeeForumsCommand = new RelayCommand(SeeForumsCommand_Execute, SeeForumsCommand_CanExecute);
            SeeMyRenovationsCommand = new RelayCommand(SeeMyRenovationsCommand_Execute, SeeMyRenovationsCommand_CanExecute);
            SeeMyProfileCommand = new RelayCommand(SeeMyProfileCommand_Execute, SeeMyProfileCommand_CanExecute);
            SeeNotificationsCommand = new RelayCommand(SeeNotificationsCommand_Execute, SeeNotificationsCommand_CanExecute);

            UpdateRenovationInformations();
            OpenReminderWindow();
            OpenNotificationWindow();
        }

        public void OpenReminderWindow()
        {
            foreach (var reservation in _accommodationReservationService.GetAllByOwnerId(_user.Id))
            {
                TimeSpan time = DateTime.Now - reservation.EndDate;

                if (time.Days > 5 || DateTime.Now < reservation.EndDate)
                {
                    continue;
                }

                int reservationId = reservation.Id;
                Accommodation foundAccommodation = _accommodationService.GetByOwnerId(_user.Id).Where(a => a.Id == reservationId).First();

                if (foundAccommodation != null)
                {
                    GuestRating foundRating = FindRating(reservation);

                    if (foundAccommodation.OwnerId == _user.Id && foundRating.Id == 0)
                    {
                        RatingGuestReminderForm ratingGuestReminderForm = new RatingGuestReminderForm();
                        ratingGuestReminderForm.ShowDialog();
                        break;
                    }
                }
            }
        }

        public GuestRating FindRating(AccommodationReservation reservation)
        {
            List<GuestRating> ratings = _guestRatingService.GetAll();
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

        public void OpenNotificationWindow()
        {
            var notifications = _accommodationNotificationService.GetAllByReceiverId(_user.Id);
            notifications = _accommodationNotificationService.GetUnseenNotifications(notifications);

            if (notifications.Count > 0)
            {
                foreach (var notification in notifications)
                {
                    NotificationWindow notificationWindow = new NotificationWindow(notification);
                    notificationWindow.ShowDialog();

                    _accommodationNotificationService.SetAsSeen(notification);
                }
            }
        }

        public void RemoveAccommodationStatusRenovated(List<AccommodationRenovation> renovations)
        {
            List<AccommodationRenovation> accommodationRenovations = renovations;

            foreach (var renovation in accommodationRenovations)
            {
                renovation.Accommodation.RenovationStatus = "";
                _accommodationService.Update(renovation.Accommodation);
            }
        }

        public void SetAccommodationStatusRenovated(List<AccommodationRenovation> finishedAndNotMarkedRenovations)
        {
            List<AccommodationRenovation> finishedRenovations = finishedAndNotMarkedRenovations;

            foreach (var renovation in finishedRenovations)
            {
                renovation.Accommodation.RenovationStatus = " RENOVATED";
                _accommodationService.Update(renovation.Accommodation);
            }
        }

        #region COMMANDS
        public RelayCommand? CloseWindowCommand { get; }
        public RelayCommand? SeeMyAccommodationsCommand { get; }
        public RelayCommand? SeeMyReservationsCommand { get; }
        public RelayCommand? SeeMyRequestsCommand { get; }
        public RelayCommand? SeeMyReviewsCommand { get; }
        public RelayCommand? SeeForumsCommand { get; }
        public RelayCommand? SeeMyRenovationsCommand { get; }
        public RelayCommand? SeeMyProfileCommand { get; }
        public RelayCommand? SeeNotificationsCommand { get; }

        public void CloseWindowCommand_Execute(object? parameter)
        {
            SignInForm signIn = new SignInForm();
            signIn.Show();
            _ownerMainWindow.Close();
        }

        public bool SeeMyAccommodationsCommand_CanExecute(object? parameter)
        {
            return SelectedPage.GetType() != typeof(MyAccommodationsPage);
        }

        public void SeeMyAccommodationsCommand_Execute(object? parameter)
        {
            SelectedPage = new MyAccommodationsPage(_user);
        }

        public bool SeeMyReservationsCommand_CanExecute(object? parameter)
        {
            return SelectedPage.GetType() != typeof(MyReservationsOverviewPage);
        }

        public void SeeMyReservationsCommand_Execute(object? parameter)
        {
            SelectedPage = new MyReservationsOverviewPage(_user);
        }

        public bool SeeMyRequestsCommand_CanExecute(object? parameter)
        {
            return SelectedPage.GetType() != typeof(RequestsOverview);
        }

        public void SeeMyRequestsCommand_Execute(object? parameter)
        {
            SelectedPage = new RequestsOverview(_user.Id);
        }

        public bool SeeMyReviewsCommand_CanExecute(object? parameter)
        {
            return SelectedPage.GetType() != typeof(RatedGuestsOverview);
        }

        public void SeeMyReviewsCommand_Execute(object? parameter)
        {
            SelectedPage = new RatedGuestsOverview(_user.Id);
        }

        public bool SeeForumsCommand_CanExecute(object? parameter)
        {
            return SelectedPage.GetType() != typeof(ForumsOverviewPage);
        }

        public void SeeForumsCommand_Execute(object? parameter)
        {
            SelectedPage = new ForumsOverviewPage(_user);
        }

        public bool SeeMyRenovationsCommand_CanExecute(object? parameter)
        {
            return SelectedPage.GetType() != typeof(MyRenovationsOverviewPage);
        }

        public void SeeMyRenovationsCommand_Execute(object? parameter)
        {
            SelectedPage = new MyRenovationsOverviewPage(_user.Id);
        }

        public bool SeeMyProfileCommand_CanExecute(object? parameter)
        {
            return SelectedPage.GetType() != typeof(MyProfilePage);
        }

        public void SeeMyProfileCommand_Execute(object? parameter)
        {
            SelectedPage = new MyProfilePage(_user.Id);
        }

        public bool SeeNotificationsCommand_CanExecute(object? parameter)
        {
            return SelectedPage.GetType() != typeof(NotificationsOverviewPage);
        }

        public void SeeNotificationsCommand_Execute(object? parameter)
        {
            SelectedPage = new NotificationsOverviewPage();
        }
        #endregion
    }
}
