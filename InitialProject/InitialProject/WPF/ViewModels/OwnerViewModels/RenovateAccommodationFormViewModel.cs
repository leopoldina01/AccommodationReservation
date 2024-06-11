using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views.Guest1Views;
using InitialProject.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class RenovateAccommodationFormViewModel : ViewModelBase
    {
        #region PROPERTIES
        private Accommodation _selectedAccommodation;
        public Accommodation SelectedAccommodation
        {
            get
            {
                return _selectedAccommodation;
            }
            set
            {
                if (_selectedAccommodation != value)
                {
                    _selectedAccommodation = value;
                    OnPropertyChanged(nameof(SelectedAccommodation));
                }
            }
        }

        private DateTime _selectedStartDate;
        public DateTime SelectedStartDate
        {
            get => _selectedStartDate;
            set
            {
                if (_selectedStartDate != value)
                {
                    _selectedStartDate = value;
                    OnPropertyChanged(nameof(SelectedStartDate));
                }
            }
        }

        private DateTime _selectedEndDate;
        public DateTime SelectedEndDate
        {
            get => _selectedEndDate;
            set
            {
                if (_selectedEndDate != value)
                {
                    _selectedEndDate = value;
                    OnPropertyChanged(nameof(SelectedEndDate));
                }
            }
        }

        private string _lenghtOfRenovation;
        public string LenghtOfRenovation
        {
            get => _lenghtOfRenovation;
            set
            {
                if (_lenghtOfRenovation != value)
                {
                    _lenghtOfRenovation = value;
                    OnPropertyChanged(nameof(LenghtOfRenovation));
                }
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private ObservableCollection<AvailableDate> _availableDates;
        public ObservableCollection<AvailableDate> AvailableDates
        {
            get => _availableDates;
            set
            {
                if (_availableDates != value)
                {
                    _availableDates = value;
                    OnPropertyChanged(nameof(AvailableDates));
                }
            }
        }

        private AvailableDate _selectedAvailableDate;
        public AvailableDate SelectedAvailableDate
        {
            get => _selectedAvailableDate;
            set
            {
                if (_selectedAvailableDate != value)
                {
                    _selectedAvailableDate = value;
                    OnPropertyChanged(nameof(SelectedAvailableDate));
                }
            }
        }

        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly AccommodationRenovationService _accommodationRenovationService;
        #endregion

        public RenovateAccommodationFormViewModel(Accommodation selectedAccommodation) 
        {
            SelectedAccommodation = selectedAccommodation;

            AvailableDates = new ObservableCollection<AvailableDate>();

            _accommodationReservationService = new AccommodationReservationService();
            _accommodationRenovationService = new AccommodationRenovationService();

            SearchDatesCommand = new RelayCommand(SearchDateCommand_Execute, SearchDateCommand_CanExecute);
            ScheduleRenovationCommand = new RelayCommand(ScheduleRenovationCommand_Execute, ScheduleRenovationCommand_CanExecute);
        }

        public bool IsValid
        {
            get
            {
                if (DateTime.Compare(SelectedStartDate, SelectedEndDate) >= 0)
                {
                    return false;
                }

                if (DateTime.Compare(DateTime.Now.Date, SelectedStartDate.Date) > 0)
                {
                    return false;
                }

                if (LenghtOfRenovation == null)
                {
                    return false;
                }

                if (LenghtOfRenovation == "")
                {
                    return false;
                }

                if ((SelectedEndDate.Date - SelectedStartDate.Date).Days < Convert.ToInt32(LenghtOfRenovation) - 1)
                {
                    return false;
                }

                return true;
            }
        }

        private void SearchAvailableDates()
        {
            AvailableDates.Clear();
            List<DateTime> allSingleDates = FindDatesBetween(SelectedStartDate, SelectedEndDate);

            List<DateTime> availableSingleDates = new List<DateTime>();

            foreach (var date in allSingleDates)
            {
                availableSingleDates.Add(date);
            }

            foreach (var date in allSingleDates)
            {
                RemoveIfUnavailable(date, availableSingleDates);
            }

            List<AvailableDate> availableDates = FindConnectedDates(availableSingleDates, SelectedEndDate);

            foreach (var date in availableDates)
            {
                AvailableDates.Add(date);
            }

            if (AvailableDates.Count() == 0)
            {
                List<AvailableDate> recommendedDates = FindRecommendedDates();

                foreach (var date in recommendedDates)
                {
                    AvailableDates.Add(date);
                }
            }
        }

        private List<DateTime> FindDatesBetween(DateTime startDate, DateTime endDate)
        {
            List<DateTime> resultingDates = new List<DateTime>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                resultingDates.Add(date);
            }

            return resultingDates;
        }

        private void RemoveIfUnavailable(DateTime date, List<DateTime> dates)
        {
            foreach (var accommodationReservation in _accommodationReservationService.GetByAccommodationId(SelectedAccommodation.Id))
            {
                if (FindDatesBetween(accommodationReservation.StartDate, accommodationReservation.EndDate).Contains(date))
                {
                    dates.Remove(date);
                }
            }

            foreach (var accommodationRenovation in _accommodationRenovationService.GetByAccommodationId(SelectedAccommodation.Id))
            {
                if (FindDatesBetween(accommodationRenovation.StartDate, accommodationRenovation.EndDate).Contains(date))
                {
                    dates.Remove(date);
                }
            }
        }

        private List<AvailableDate> FindConnectedDates(List<DateTime> singleDates, DateTime finishEndDate)
        {
            List<AvailableDate> connectedDates = new List<AvailableDate>();

            foreach (var singleDate in singleDates)
            {
                AvailableDate newDate = new AvailableDate(singleDate, singleDate.AddDays(Convert.ToInt32(LenghtOfRenovation) - 1));

                foreach (var date in FindDatesBetween(newDate.StartDate, newDate.EndDate))
                {
                    if (!singleDates.Contains(date))
                    {
                        break;
                    }
                    else if (date == newDate.EndDate)
                    {
                        if (DateTime.Compare(newDate.EndDate, finishEndDate) <= 0)
                        {
                            connectedDates.Add(newDate);
                        }
                    }
                }
            }

            return connectedDates;
        }

        private List<AvailableDate> FindRecommendedDates()
        {
            DateTime recommendationStartDate;

            if ((SelectedStartDate.Date - DateTime.Now.Date).Days < 14)
            {
                recommendationStartDate = DateTime.Now.Date;
            }
            else
            {
                recommendationStartDate = SelectedStartDate.AddDays(-14);
            }

            DateTime recommendationEndDate = SelectedEndDate.AddDays(14);
            List<DateTime> singleDates = FindDatesBetween(recommendationStartDate, recommendationEndDate);
            List<DateTime> availableSingleDates = FindDatesBetween(recommendationStartDate, recommendationEndDate);

            foreach (var date in singleDates)
            {
                RemoveIfUnavailable(date, availableSingleDates);
            }

            List<AvailableDate> recommendedDays = FindConnectedDates(availableSingleDates, recommendationEndDate);

            return recommendedDays;
        }

        #region COMMANDS
        public RelayCommand SearchDatesCommand { get; }
        public RelayCommand ScheduleRenovationCommand { get; }
        public RelayCommand SeeAllCommand { get; }

        public bool SearchDateCommand_CanExecute(object? parameter)
        {
            return IsValid;
        }

        public void SearchDateCommand_Execute(object? parameter)
        {
            SearchAvailableDates();
        }

        public bool ScheduleRenovationCommand_CanExecute(object? parameter)
        {
            return SelectedAvailableDate != null;
        }

        public void ScheduleRenovationCommand_Execute(object? parameter)
        {
            AccommodationRenovation accommodationRenovation = _accommodationRenovationService.Save(SelectedAccommodation, SelectedAvailableDate.StartDate, SelectedAvailableDate.EndDate, Convert.ToInt32(LenghtOfRenovation), Description);
        }

        public void SeeAllCommand_Execute(object? parameter)
        {
        }
        #endregion
    }

    public class AvailableDate
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AvailableDate() { }
        public AvailableDate(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
