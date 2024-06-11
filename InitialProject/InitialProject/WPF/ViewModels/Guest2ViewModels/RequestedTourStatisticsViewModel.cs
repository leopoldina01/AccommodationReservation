using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using InitialProject.Commands;
using InitialProject.WPF.Views.Guest2Views;
using InitialProject.WPF.Views;
using InitialProject.Application.UseCases;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using InitialProject.Domain.DTOs;
using HarfBuzzSharp;
using System.Windows.Documents;
using System.Diagnostics.Metrics;
using System.Windows.Controls;
using System.Windows.Forms;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class RequestedTourStatisticsViewModel : ViewModelBase
    {
        #region PROPERTIES
        User LoggedUser { get; set; }

        private ObservableCollection<string> _years;
        public ObservableCollection<string> Years
        {
            get => _years;
            set
            {
                if (value != _years)
                {
                    _years = value;
                    OnPropertyChanged(nameof(Years));
                }
            }
        }
        
        private ObservableCollection<LanguageStatistics> _languageStats;
        public ObservableCollection<LanguageStatistics> LanguageStats
        {
            get => _languageStats;
            set
            {
                if (value != _languageStats)
                {
                    _languageStats = value;
                    OnPropertyChanged(nameof(LanguageStats));
                }
            }
        }

        private ObservableCollection<LocationStatistics> _locationStats;
        public ObservableCollection<LocationStatistics> LocationStats
        {
            get => _locationStats;
            set
            {
                if (value != _locationStats)
                {
                    _locationStats = value;
                    OnPropertyChanged(nameof(LocationStats));
                }
            }
        }

        private List<string> _languages;
        public List<string> Languages
        {
            get => _languages;
            set
            {
                if (value != _languages)
                {
                    _languages = value;
                    OnPropertyChanged(nameof(Languages));
                }
            }
        }

        private string _selectedYear;
        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged(nameof(SelectedYear));
                    SelectionChangedYears();
                }
            }
        }

        

        private string _allYears;
        public string AllYears
        {
            get => _allYears;
            set
            {
                if (_allYears != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged(nameof(AllYears));
                }
            }
        }

        private double _approvedReqPercentage;
        public double ApprovedReqPercentage
        {
            get => _approvedReqPercentage;
            set
            {
                if (_approvedReqPercentage != value)
                {
                    _approvedReqPercentage = value;
                    OnPropertyChanged(nameof(ApprovedReqPercentage));
                }
            }
        }

        private double _declinedReqPercentage;
        public double DeclinedReqPercentage
        {
            get => _declinedReqPercentage;
            set
            {
                if (_declinedReqPercentage != value)
                {
                    _declinedReqPercentage = value;
                    OnPropertyChanged(nameof(DeclinedReqPercentage));
                }
            }
        }

        private double _averageGuestNumber;
        public double AverageGuestNumber
        {
            get => _averageGuestNumber;
            set
            {
                if (_averageGuestNumber != value)
                {
                    _averageGuestNumber = value;
                    OnPropertyChanged(nameof(AverageGuestNumber));
                }
            }
        }

        private readonly TourRequestService _tourRequestService;
        private readonly LocationService _locationService;
        private readonly TourRequestStatisticsService _tourRequestStatisticsService;

        #endregion
        public RequestedTourStatisticsViewModel(User loggedUser)
        {
            LoggedUser = loggedUser;
            _tourRequestService = new TourRequestService();
            _locationService = new LocationService();
            _tourRequestStatisticsService = new TourRequestStatisticsService();
            LanguageStats = new ObservableCollection<LanguageStatistics>();
            LocationStats = new ObservableCollection<LocationStatistics>();
            Years = new ObservableCollection<string>();

            LoadInitialStatistics();
        }

        public void LoadInitialStatistics()
        {
            LoadLanguageStats();
            LoadLocationStats();
            LoadYears();
            LoadAllYearsStats();
            AllYears = "All Years";
        }


        public void LoadYears()
        {
            Years.Add("All Years");
            foreach(var req in _tourRequestService.GetAll())
            {
                if(!Years.Contains(req.StartDate.Year.ToString()))
                {
                    Years.Add(req.StartDate.Year.ToString());
                }
            }
        }

        public void LoadLanguageStats()
        {
            foreach(var language in GetAllLanguages())
            {
                double counter = 0;
                foreach (var req in _tourRequestService.GetAll())
                {
                    if(language == req.Language)
                    {
                        counter++;
                    } 
                }
                if(counter != 0)
                {
                    LanguageStatistics languageStatistics = new LanguageStatistics(language, counter);
                    LanguageStats.Add(languageStatistics);
                }
            }
        }
        public void LoadLocationStats()
        {
            foreach (var locationStat in _tourRequestStatisticsService.GetLocationStats())
            {
                LocationStats.Add(locationStat);
            }
                
        }

        public void SelectionChangedYears()
        {
            if(SelectedYear == "All Years")
            {
                LoadAllYearsStats();
            }
            else
            {
                LoadApprovedPercentage(SelectedYear);
                LoadDeclinedPercentage(SelectedYear);
                LoadAvgGuestNumber(SelectedYear);
            }
        }

        public void LoadAllYearsStats()
        {
            var acceptedCounter = 0;
            var declinedCounter = 0;
            double requestCounter = 0;
            double guestCounter = 0;

            foreach(var req in _tourRequestService.GetAll())
            {
                requestCounter++;
                guestCounter = guestCounter + req.GuestsNumber;
                if (req.Status == TourRequestStatus.ACCEPTED)
                {
                    acceptedCounter++;
                }
                else if (req.Status==TourRequestStatus.DECLINED)
                {
                    declinedCounter++;
                }
            }

            AverageGuestNumber = Math.Round(guestCounter/requestCounter, 2);
            ApprovedReqPercentage = Math.Round((100*acceptedCounter)/requestCounter, 2);
            DeclinedReqPercentage = Math.Round((100*declinedCounter)/requestCounter, 2);
            
        }

        public void LoadAvgGuestNumber(string selectedYear)
        {
            double guestCounter = 0;
            double requestCounter = 0;
            foreach (var req in _tourRequestService.GetAll())
            {
                if (req.StartDate.Year.ToString() == selectedYear)
                {
                    requestCounter++;
                    guestCounter = guestCounter + req.GuestsNumber;
                }
            }
            AverageGuestNumber = Math.Round(guestCounter/requestCounter, 2);
        }

        public void LoadDeclinedPercentage(string selectedYear)
        {
            var declinedCounter = 0;
            double requestCounter = 0;
            foreach (var req in _tourRequestService.GetAll())
            {
                if (req.StartDate.Year.ToString() == selectedYear)
                {
                    requestCounter++;
                    if(req.Status == TourRequestStatus.DECLINED)
                    {
                        declinedCounter++;
                    }
                }
            }
            DeclinedReqPercentage = Math.Round((100*declinedCounter)/requestCounter, 2);
        }

        public void LoadApprovedPercentage(string selectedYear)
        {
            var acceptedCounter = 0;
            double requestCounter = 0;
            foreach (var req in _tourRequestService.GetAll())
            {
                if (req.StartDate.Year.ToString() == selectedYear)
                {
                    requestCounter++;
                    if (req.Status == TourRequestStatus.ACCEPTED)
                    {
                        acceptedCounter++;
                    }
                }
            }
            ApprovedReqPercentage = Math.Round((100*acceptedCounter)/requestCounter, 2);
        }

        public List<string> GetAllLanguages()
        {
            return Languages = new List<string> { "Serbian", "Hungarian", "German", "Thai", "French", "Italian", "Turkish", "Chinese", "Bulgarian", "Swedish", "Finish", "Croatian", "Bosnian", "Japanese", "Eren Yeager", "Danish", "English", "Romanian", "Greek", "Albanian", "Ukranian", "Russian", "Slovenian", "Slovakian", "Belgian", "Dutch", "Portuguese", "Spanish", "Lithuanian", "Estonian" };
        }

        #region COMMANDS
        public RelayCommand HomeCommand { get; }

        public void HomeCommand_Execute(object? parameter)
        {
            Guest2TourView guest2TourView = new Guest2TourView(LoggedUser);
        }
        #endregion
    }
}
