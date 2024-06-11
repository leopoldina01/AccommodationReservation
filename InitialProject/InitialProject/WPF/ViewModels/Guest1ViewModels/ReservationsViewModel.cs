using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views.Guest1Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class ReservationsViewModel : ViewModelBase
    {
        #region PROPERTIES
        private AccommodationReservation? _selectedReservation;
        public AccommodationReservation? SelectedReservation
        {
            get
            {
                return _selectedReservation;
            }
            set
            {
                if (_selectedReservation != value)
                {
                    _selectedReservation = value;
                    OnPropertyChanged(nameof(SelectedReservation));
                }
            }
        }

        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        public User LoggedUser { get; set; }

        private readonly int _guestId;
        private readonly Window _reservationsView;

        private readonly AccommodationReservationService _reservationService;
        private readonly AccommodationRatingService _ratingService;
        private readonly AccommodationNotificationService _accommodationNotificationService;
        private readonly AccommodationYearStatisticsService _accommodationYearStatisticsService;
        private readonly AccommodationMonthStatisticsService _accommodationMonthStatisticsService;
        private readonly ReservationRequestService _reservationRequestService;
        private readonly UserService _userService;
        #endregion

        public ReservationsViewModel(int guestId)
        { 
            _reservationService = new AccommodationReservationService();
            _ratingService = new AccommodationRatingService();
            _accommodationNotificationService = new AccommodationNotificationService();
            _accommodationYearStatisticsService = new AccommodationYearStatisticsService();
            _accommodationMonthStatisticsService = new AccommodationMonthStatisticsService();
            _reservationRequestService = new ReservationRequestService();
            _userService = new UserService();

            _guestId = guestId;
            LoggedUser = _userService.GetById(_guestId);

            Reservations = new ObservableCollection<AccommodationReservation>();

            CancelReservationCommand = new RelayCommand(CancelReservationCommand_Execute, CancelReservationCommand_CanExecute);
            RateYourStayCommand = new RelayCommand(RateYourStayCommand_Execute, RateYourStayCommand_CanExecute);
            ChangeReservationCommand = new RelayCommand(ChangeReservationCommand_Execute, ChangeReservationCommand_CanExecute);
            ViewAllChangeRequestsCommand = new RelayCommand(ViewAllChangeRequestsCommand_Execute);
            GenerateReportCommand = new RelayCommand(GenerateReportCommand_Execute);

            LoadReservations();
        }

        public void LoadReservations()
        {
            Reservations.Clear();
            foreach (var reservation in _reservationService.GetGuestsReservations(_guestId))
            {
                Reservations.Add(reservation);
            }
        }

        private void SaveMonthStatistics(AccommodationYearStatistic yearStatistic)
        {
            for (int i = 1; i <= 12; i++)
            {
                if (i == DateTime.Now.Month)
                {
                    _accommodationMonthStatisticsService.Save(i, yearStatistic, yearStatistic.Id, 0, 1, 0, 0);
                }

                _accommodationMonthStatisticsService.Save(i, yearStatistic, yearStatistic.Id, 0, 0, 0, 0);
            }
        }

        private bool IsReservationRated()
        {
            return _ratingService.FindAccommodationRatingByReservationId(SelectedReservation.Id) != null;
        }

        #region COMMANDS
        public RelayCommand ChangeReservationCommand { get; }
        public RelayCommand CancelReservationCommand { get; }
        public RelayCommand ViewAllChangeRequestsCommand { get; }
        public RelayCommand RateYourStayCommand { get; }
        public RelayCommand GenerateReportCommand { get; }

        public void CancelReservationCommand_Execute(object? parameter)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel your reservation?", "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _reservationService.CancelReservation(SelectedReservation);
                _accommodationNotificationService.NotifyUser($"{SelectedReservation.Guest.Username} has cancelled the reservation for {SelectedReservation.Accommodation.Name}.", _guestId, SelectedReservation.Accommodation.OwnerId);

                AccommodationYearStatistic yearStatistic = _accommodationYearStatisticsService.FindStatisticForYearAndAccommodation(SelectedReservation.Accommodation.Id, DateTime.Now.Year);

                if (yearStatistic == null)
                {
                    yearStatistic = _accommodationYearStatisticsService.Save(DateTime.Now.Year, SelectedReservation.Accommodation, SelectedReservation.Accommodation.Id, 0, 1, 0, 0);
                }
                else
                {
                    yearStatistic.NumberOfDeclinedReservations++;
                    _accommodationYearStatisticsService.Update(yearStatistic);
                }

                AccommodationMonthStatistics monthStatistics = _accommodationMonthStatisticsService.FindStatisticForMonthByYearStatistic(yearStatistic, DateTime.Now.Month);

                if (monthStatistics == null)
                {
                    SaveMonthStatistics(yearStatistic);
                }
                else
                {
                    monthStatistics.NumberOfDeclinedReservations++;
                    _accommodationMonthStatisticsService.Update(monthStatistics);
                }

                _reservationRequestService.CancelRequest(SelectedReservation.Id);

                LoadReservations();
            }
        }

        public bool CancelReservationCommand_CanExecute(object? parameter)
        {
            return SelectedReservation is not null && (SelectedReservation.StartDate - DateTime.Now.Date).Days >= SelectedReservation.Accommodation.MinDaysBeforeCancel;
        }

        public void RateYourStayCommand_Execute(object? parameter)
        {
            MainWindow.mainWindow.MainPreview.Content = new AccommodationRatingFormPage(new AccommodationRatingFormViewModel(_reservationsView, SelectedReservation));
        }

        public bool RateYourStayCommand_CanExecute(object? parameter)
        {
            return SelectedReservation is not null && (DateTime.Now - SelectedReservation.EndDate).Days < 6 
                && DateTime.Compare(DateTime.Now.Date, SelectedReservation.EndDate.Date) >= 0
                && !IsReservationRated();
        }

        public void ChangeReservationCommand_Execute(object? parameter)
        {
            MainWindow.mainWindow.MainPreview.Content = new ReservationChangePage(new ReservationChangeViewModel(_reservationsView, SelectedReservation));
        }

        public bool ChangeReservationCommand_CanExecute(object? parameter)
        {
            return SelectedReservation is not null && (SelectedReservation.StartDate - DateTime.Now.Date).Days > 0;
        }

        public void ViewAllChangeRequestsCommand_Execute(object? parameter)
        {
            MainWindow.mainWindow.MainPreview.Content = new ReservationChangeRequestsPage(new ReservationChangeRequestsViewModel(_reservationsView, _guestId));
        }

        public void GenerateReportCommand_Execute(object? parameter)
        {
            MainWindow.mainWindow.MainPreview.Content = new GenerateReportPage(LoggedUser);
        }
#endregion
    }
}
