using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views.Guest1Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class RenovationSuggestionViewModel : ViewModelBase
    {
        #region PROPERTIES
        private string _comment;
        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged(nameof(Comment));
                }
            }
        }

        private bool[] _urgencyModeArray = new bool[] { true, false, false, false, false };

        public bool[] UrgencyModeArray
        {
            get { return _urgencyModeArray; }
        }

        public int UrgencySelectedMode
        {
            get { return Array.IndexOf(_urgencyModeArray, true);  }
        }

        public AccommodationReservation SelectedReservation { get; }

        private readonly AccommodationRenovationSuggestionService _accommodationRenovationSuggestionService;
        private readonly AccommodationMonthStatisticsService _accommodationMonthStatisticsService;
        private readonly AccommodationYearStatisticsService _accommodationYearStatisticsService;
        #endregion

        public RenovationSuggestionViewModel(AccommodationReservation reservation)
        {
            _accommodationRenovationSuggestionService = new AccommodationRenovationSuggestionService();
            _accommodationMonthStatisticsService = new AccommodationMonthStatisticsService();
            _accommodationYearStatisticsService = new AccommodationYearStatisticsService();

            SelectedReservation = reservation;

            SubmitReviewCommand = new RelayCommand(SubmitReviewCommand_Execute);
        }

        private void SaveMonthStatistics(AccommodationYearStatistic yearStatistic)
        {
            for (int i = 1; i <= 12; i++)
            {
                if (i == DateTime.Now.Month)
                {
                    _accommodationMonthStatisticsService.Save(i, yearStatistic, yearStatistic.Id, 0, 0, 0, 1);
                }

                _accommodationMonthStatisticsService.Save(i, yearStatistic, yearStatistic.Id, 0, 0, 0, 0);
            }
        }

        #region COMMANDS
        public RelayCommand SubmitReviewCommand { get; }

        public void SubmitReviewCommand_Execute(object? parameter)
        {
            _accommodationRenovationSuggestionService.SaveSuggestion(Comment, UrgencySelectedMode + 1, SelectedReservation.Id, SelectedReservation.Accommodation.OwnerId, SelectedReservation.GuestId);

            AccommodationYearStatistic yearStatistic = _accommodationYearStatisticsService.FindStatisticForYearAndAccommodation(SelectedReservation.Accommodation.Id, DateTime.Now.Year);

            if (yearStatistic == null)
            {
                yearStatistic = _accommodationYearStatisticsService.Save(DateTime.Now.Year, SelectedReservation.Accommodation, SelectedReservation.Accommodation.Id, 0, 0, 0, 1);
            }
            else
            {
                yearStatistic.NumberOfRenovationSuggestions++;
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
        #endregion
    }
}
