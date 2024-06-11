using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views.Guest1Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class ReservationChangeViewModel : ViewModelBase
    {
        #region PROPERTIES
        public AccommodationReservation SelectedReservation { get; set; }
        private DateTime _selectedStartDate;
        public DateTime SelectedStartDate
        {
            get
            {
                return _selectedStartDate;
            }
            set
            {
                if (value != _selectedStartDate)
                {
                    _selectedStartDate = value;
                    OnPropertyChanged(nameof(SelectedStartDate));
                }
            }
        }
        private DateTime _selectedEndDate;
        public DateTime SelectedEndDate
        {
            get
            {
                return _selectedEndDate;
            }
            set
            {
                if (value != _selectedEndDate)
                {
                    _selectedEndDate = value;
                    OnPropertyChanged(nameof(SelectedEndDate));
                }
            }
        }

        private readonly Window _reservationChangeView;

        private readonly ReservationRequestService _requestService;
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly AccommodationYearStatisticsService _accommodationYearStatisticsService;
        private readonly AccommodationMonthStatisticsService _accommodationMonthStatisticsService;
        #endregion

        public ReservationChangeViewModel(Window reservationChangeView, AccommodationReservation selectedReservation)
        {
            _reservationChangeView = reservationChangeView;

            _requestService = new ReservationRequestService();
            _accommodationReservationService = new AccommodationReservationService();
            _accommodationYearStatisticsService = new AccommodationYearStatisticsService();
            _accommodationMonthStatisticsService = new AccommodationMonthStatisticsService();


            SelectedReservation = selectedReservation;

            RequestDateChangeCommand = new RelayCommand(RequestDateChangeCommand_Execute, RequestDateChangeCommand_CanExecute);
        }

        private void SaveMonthStatistics(AccommodationYearStatistic yearStatistic)
        {
            for (int i = 1; i <= 12; i++)
            {
                if (i == DateTime.Now.Month)
                {
                    _accommodationMonthStatisticsService.Save(i, yearStatistic, yearStatistic.Id, 0, 0, 1, 0);
                }

                _accommodationMonthStatisticsService.Save(i, yearStatistic, yearStatistic.Id, 0, 0, 0, 0);
            }
        }

        #region COMMANDS
        public RelayCommand RequestDateChangeCommand { get; }
       
        public void RequestDateChangeCommand_Execute(object? parameter)
        {
            _requestService.CreateRequest(SelectedStartDate, SelectedEndDate, SelectedReservation);

            AccommodationYearStatistic yearStatistic = _accommodationYearStatisticsService.FindStatisticForYearAndAccommodation(SelectedReservation.Accommodation.Id, DateTime.Now.Year);

            if (yearStatistic == null)
            {
                yearStatistic = _accommodationYearStatisticsService.Save(DateTime.Now.Year, SelectedReservation.Accommodation, SelectedReservation.Accommodation.Id, 0, 0, 1, 0);
            }
            else
            {
                yearStatistic.NumberOfChangedReservations++;
                _accommodationYearStatisticsService.Update(yearStatistic);
            }

            AccommodationMonthStatistics monthStatistics = _accommodationMonthStatisticsService.FindStatisticForMonthByYearStatistic(yearStatistic, DateTime.Now.Month);

            if (monthStatistics == null)
            {
                SaveMonthStatistics(yearStatistic);
            }
            else
            {
                monthStatistics.NumberOfChangedReservations++;
                _accommodationMonthStatisticsService.Update(monthStatistics);
            }

            MainWindow.mainWindow.MainPreview.Content = new ReservationsPage(new ReservationsViewModel(SelectedReservation.GuestId));
        }

        public bool RequestDateChangeCommand_CanExecute(object? parameter)
        {
            return DateTime.Compare(SelectedStartDate, SelectedEndDate) < 0
                && DateTime.Compare(DateTime.Now.Date, SelectedStartDate.Date) <= 0
                && (SelectedEndDate.Date - SelectedStartDate.Date).Days + 1 >= SelectedReservation.Accommodation.MinDaysForStay;
        }
        #endregion
    }
}
