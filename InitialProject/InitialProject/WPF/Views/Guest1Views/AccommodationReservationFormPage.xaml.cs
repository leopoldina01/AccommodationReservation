using InitialProject.Application.UseCases;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for AccommodationReservationFormPage.xaml
    /// </summary>
    public partial class AccommodationReservationFormPage : Page, INotifyPropertyChanged
    {
        public readonly AccommodationRepository _accommodationRepository;
        public readonly AccommodationImageRepository _accommodationImageRepository;
        public readonly LocationRepository _locationRepository;
        public readonly AccommodationReservationRepository _accommodationReservationRepository;
        public readonly UserRepository _userRepository;

        private readonly AccommodationRenovationService _accommodationRenovationService;
        private readonly AccommodationYearStatisticsService _accommodationYearStatisticsService;
        private readonly AccommodationMonthStatisticsService _accommodationMonthStatisticsService;

        public User LoggedUser { get; set; }

        private Accommodation _accommodation;
        public Accommodation SelectedAccommodation
        {
            get => _accommodation;
            set
            {
                if (_accommodation != value)
                {
                    _accommodation = value;
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

        private string _lenghtOfStay;
        public string LenghtOfStay
        {
            get => _lenghtOfStay;
            set
            {
                if (_lenghtOfStay != value)
                {
                    _lenghtOfStay = value;
                    OnPropertyChanged(nameof(LenghtOfStay));
                }
            }
        }

        private string _guestNumber;
        public string GuestNumber
        {
            get => _guestNumber;
            set
            {
                if (_guestNumber != value)
                {
                    _guestNumber = value;
                    OnPropertyChanged(nameof(GuestNumber));
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

        private AccommodationImage _currentImage;
        public AccommodationImage CurrentImage
        {
            get => _currentImage;
            set
            {
                if (_currentImage != value)
                {
                    _currentImage = value;
                    OnPropertyChanged(nameof(CurrentImage));
                }
            }
        }
        public List<AccommodationImage> AccommodationImages { get; set; }
        private readonly SuperGuestService _superGuestService;

        private Regex _NaturalNumberRegex = new Regex("^[1-9][0-9]*$");
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public AccommodationReservationFormPage(User user, AccommodationRepository accommodationRepository, LocationRepository locationRepository, AccommodationImageRepository accommodationImageRepository, AccommodationReservationRepository accommodationReservationRepository, UserRepository userRepository,Accommodation selectedAccommodation)
        {
            InitializeComponent();
            DataContext = this;

            _accommodationRepository = accommodationRepository;
            _locationRepository = locationRepository;
            _accommodationImageRepository = accommodationImageRepository;
            _accommodationReservationRepository = accommodationReservationRepository;
            _userRepository = userRepository;

            _superGuestService = new SuperGuestService();

            _accommodationRenovationService = new AccommodationRenovationService();
            _accommodationYearStatisticsService = new AccommodationYearStatisticsService();
            _accommodationMonthStatisticsService = new AccommodationMonthStatisticsService();

            LoggedUser = user;
            SelectedAccommodation = selectedAccommodation;
            AvailableDates = new ObservableCollection<AvailableDate>();
            AccommodationImages = FindSelectedAccommodationImages();
            CurrentImage = AccommodationImages[0];
            if (AccommodationImages.Count > 1)
            {
                ButtonNextImage.IsEnabled = true;
                ButtonNextImage.Opacity = 100;
            }
        }

        private void SearchDatesButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsDateSearchInputValid()) return;

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
                MessageBox.Show("There are no available days for selected dates.\n" +
                        "Available dates around selected date are shown instead.");
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
            foreach (var accommodationReservation in _accommodationReservationRepository.GetAll())
            {
                if (FindDatesBetween(accommodationReservation.StartDate, accommodationReservation.EndDate).Contains(date) && accommodationReservation.AccommodationId == SelectedAccommodation.Id)
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
                AvailableDate newDate = new AvailableDate(singleDate, singleDate.AddDays(Convert.ToInt32(LenghtOfStay) - 1));
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

        private bool IsDateSearchInputValid()
        {
            if (DateTime.Compare(SelectedStartDate, SelectedEndDate) >= 0)
            {
                MessageBox.Show("First date must be before the second one.");
                return false;
            }

            if (DateTime.Compare(DateTime.Now.Date, SelectedStartDate.Date) > 0)
            {
                MessageBox.Show("You can't travel to the past.");
                return false;
            }

            if (LenghtOfStay == null || LenghtOfStay == "")
            {
                MessageBox.Show("Duration of stay field can't be empty.");
                return false;
            }

            if (!_NaturalNumberRegex.Match(LenghtOfStay).Success)
            {
                MessageBox.Show("Please enter a valid value.");
                return false;
            }

            if ((SelectedEndDate.Date - SelectedStartDate.Date).Days < Convert.ToInt32(LenghtOfStay) - 1)
            {
                MessageBox.Show("Calendar and Duration of stay values don't match.");
                return false;
            }

            if (Convert.ToInt32(LenghtOfStay) < SelectedAccommodation.MinDaysForStay)
            {
                MessageBox.Show("Minimum days for stay is " + SelectedAccommodation.MinDaysForStay + ".");
                return false;
            }

            return true;
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

        private void MakeReservationButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsMakeReservationInputValid()) return;

            _accommodationReservationRepository.Save(SelectedAvailableDate.StartDate, SelectedAvailableDate.EndDate, Convert.ToInt32(LenghtOfStay), SelectedAccommodation, SelectedAccommodation.Id, LoggedUser, LoggedUser.Id, "Scheduled");

            AccommodationYearStatistic yearStatistic = _accommodationYearStatisticsService.FindStatisticForYearAndAccommodation(SelectedAccommodation.Id, DateTime.Now.Year);

            if (yearStatistic == null)
            {
                yearStatistic = _accommodationYearStatisticsService.Save(DateTime.Now.Year, SelectedAccommodation, SelectedAccommodation.Id, 1, 0, 0, 0);
            }
            else
            {
                yearStatistic.NumberOfReservations++;
                _accommodationYearStatisticsService.Update(yearStatistic);
            }

            AccommodationMonthStatistics monthStatistics = _accommodationMonthStatisticsService.FindStatisticForMonthByYearStatistic(yearStatistic, DateTime.Now.Month);

            if (monthStatistics == null)
            {
                SaveMonthStatistics(yearStatistic);
            }
            else
            {
                monthStatistics.NumberOfReservations++;
                _accommodationMonthStatisticsService.Update(monthStatistics);
            }

            _superGuestService.RemoveBonusPoint(LoggedUser.Id);
            MainWindow.mainWindow.MainPreview.Content = new AccommodationsPage(LoggedUser, _accommodationRepository, _locationRepository, _accommodationImageRepository, _accommodationReservationRepository, _userRepository);
            MessageBox.Show("Reservation for " + SelectedAccommodation.Name + " (" + LenghtOfStay + " days) is successfully made!");
        }

        private void SaveMonthStatistics(AccommodationYearStatistic yearStatistic)
        {
            for (int i = 1; i <= 12; i++)
            {
                if (i == DateTime.Now.Month)
                {
                    _accommodationMonthStatisticsService.Save(i, yearStatistic, yearStatistic.Id, 1, 0, 0, 0);
                }
                else
                {
                    _accommodationMonthStatisticsService.Save(i, yearStatistic, yearStatistic.Id, 0, 0, 0, 0);
                }

                
            }
        }

        private bool IsMakeReservationInputValid()
        {
            if (GuestNumber == null || GuestNumber == "")
            {
                MessageBox.Show("Number of guests field can't be empty.");
                return false;
            }

            if (!_NaturalNumberRegex.Match(GuestNumber).Success)
            {
                MessageBox.Show("Please enter a valid value.");
                return false;
            }

            if (Convert.ToInt32(GuestNumber) > SelectedAccommodation.Capacity)
            {
                MessageBox.Show("Maximum capacity is " + SelectedAccommodation.Capacity + ".");
                return false;
            }

            if (SelectedAvailableDate == null)
            {
                MessageBox.Show("Please select date for the reservation.");
                return false;
            }

            return true;
        }

        private List<AccommodationImage> FindSelectedAccommodationImages()
        {
            List<AccommodationImage> accommodationImages = new List<AccommodationImage>();
            foreach (var image in _accommodationImageRepository.GetAll())
            {
                if (image.AccommodationId == SelectedAccommodation.Id)
                {
                    accommodationImages.Add(image);
                }
            }
            return accommodationImages;
        }
        private int GetImageIndex()
        {
            return AccommodationImages.IndexOf(CurrentImage);
        }

        private void RightArrowButton_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = GetImageIndex();
            var isSecondToLastImage = currentIndex == AccommodationImages.Count - 2;

            if (isSecondToLastImage)
            {
                DisableButton(ButtonNextImage);
            }
            EnableButton(ButtonPreviousImage);

            CurrentImage = AccommodationImages[currentIndex + 1];
        }

        private void LeftArrowButton_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = GetImageIndex();
            var isSecondImage = currentIndex == 1;
            if (isSecondImage)
            {
                DisableButton(ButtonPreviousImage);
            }
            EnableButton(ButtonNextImage);

            CurrentImage = AccommodationImages[currentIndex - 1];
        }
        private void EnableButton(Button button)
        {
            button.IsEnabled = true;
            button.Opacity = 100;
        }
        private void DisableButton(Button button)
        {
            button.IsEnabled = false;
            button.Opacity = 0;
        }
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
