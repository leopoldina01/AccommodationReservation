using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.PDF;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class AccommodationMonthlyStatisticsOverviewViewModel : ViewModelBase
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


        private AccommodationYearStatistic _selectedYearStatistics;
        public AccommodationYearStatistic SelectedYearStatistics
        {
            get
            {
                return _selectedYearStatistics;
            }
            set
            {
                if (_selectedYearStatistics != value)
                {
                    _selectedYearStatistics = value;
                    OnPropertyChanged(nameof(SelectedYearStatistics));
                }
            }
        }

        private int _mostTakenMonth;
        public int MostTakenMonth
        {
            get
            {
                return _mostTakenMonth;
            }
            set
            {
                if (_mostTakenMonth != value)
                {
                    _mostTakenMonth = value;
                    OnPropertyChanged(nameof(MostTakenMonth));
                }

            }
        }

        public ObservableCollection<AccommodationMonthStatistics> AccommodationMonthStatistics { get; set; }

        private readonly AccommodationMonthStatisticsService _accommodationMonthStatisticsService;
        private readonly AccommodationReservationService _accommodationReservationService;

        private readonly UserService _userService;

        public string[] Labels { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public Func<int, string> Formatter { get; set; }

        private HashSet<int> formattedValues = new HashSet<int>();

        private double? previousFormattedValue = null;

        private User _owner;
        #endregion

        public AccommodationMonthlyStatisticsOverviewViewModel(Accommodation selectedAccommodation, AccommodationYearStatistic selectedYearStatistic)
        {
            SelectedAccommodation = selectedAccommodation;
            SelectedYearStatistics = selectedYearStatistic;

            AccommodationMonthStatistics = new ObservableCollection<AccommodationMonthStatistics>();

            _accommodationMonthStatisticsService = new AccommodationMonthStatisticsService();
            _accommodationReservationService = new AccommodationReservationService();
            _userService = new UserService();

            _owner = _userService.GetById(selectedAccommodation.OwnerId);

            LoadLabels();
            LoadColumns();
            LoadMonthStatistics();
            FindMostTakenMonth();

            CreatePDFForMonthsCommand = new RelayCommand(CreatePDFForMonthsCommand_Execute);
        }

        private void LoadLabels()
        {
            Labels = new[] { "Janyary", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        }

        private void LoadColumns()
        {
            SeriesCollection = new SeriesCollection() {
                new ColumnSeries
                {
                    Title = "Reservations",
                    Values = new ChartValues<int>(),
                    ColumnPadding = -10
                },
                new ColumnSeries
                {
                    Title = "Canceled Reservations",
                    Values = new ChartValues<int>(),
                    ColumnPadding = -10
                },
                new ColumnSeries
                {
                    Title = "Requests",
                    Values = new ChartValues<int>(),
                    ColumnPadding = -10
                },
                new ColumnSeries
                {
                    Title = "Suggestions",
                    Values = new ChartValues<int>(),
                    ColumnPadding = -10
                }
            };
        }

        private void LoadMonthStatistics()
        {
            foreach (var monthStatistic in _accommodationMonthStatisticsService.GetAllByYearStatistic(SelectedYearStatistics.Id))
            {
                AccommodationMonthStatistics.Add(monthStatistic);

                SeriesCollection[0].Values.Add(monthStatistic.NumberOfReservations);
                SeriesCollection[1].Values.Add(monthStatistic.NumberOfDeclinedReservations);
                SeriesCollection[2].Values.Add(monthStatistic.NumberOfChangedReservations);
                SeriesCollection[3].Values.Add(monthStatistic.NumberOfRenovationSuggestions);
            }

            Formatter = value =>
            {
                if (!formattedValues.Contains(value))
                {
                    formattedValues.Add(value);
                    return value.ToString("N0");
                }
                else
                {
                    return string.Empty;
                }
            };
        }

        private void FindMostTakenMonth()
        {
            MostTakenMonth = 1;
            int maxNumberOfTakenDays = 0;

            foreach (var monthStatistics in _accommodationMonthStatisticsService.GetAllByYearStatistic(SelectedYearStatistics.Id))
            {
                int numberOfTakenDaysInMonth = _accommodationReservationService.GetNumberOfTakenDaysInMonthByAccommodationId(monthStatistics.Month, SelectedYearStatistics);

                if (numberOfTakenDaysInMonth > maxNumberOfTakenDays)
                {
                    maxNumberOfTakenDays = numberOfTakenDaysInMonth;
                    MostTakenMonth = monthStatistics.Month;
                }
            }
            
        }

        #region COMMANDS
        public RelayCommand CreatePDFForMonthsCommand { get; }

        public void CreatePDFForMonthsCommand_Execute(object? parameter)
        {
            AccommodationMonthStatisticsPDFCreator pdfCreator = new AccommodationMonthStatisticsPDFCreator(_accommodationMonthStatisticsService, SelectedYearStatistics, SelectedAccommodation);
            pdfCreator.CreatePDF(_owner);
            //System.Diagnostics.Process.Start("explorer", "monthStatistics.pdf");
            string absolutePath = AppDomain.CurrentDomain.BaseDirectory + $"../../../Reports/monthStatistics.pdf";
            Process.Start(new ProcessStartInfo
            {
                FileName = absolutePath,
                UseShellExecute = true
            });
        }
        #endregion
    }
}
