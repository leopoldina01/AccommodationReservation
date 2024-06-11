using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.PDF;
using InitialProject.WPF.Views.OwnerViews;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class AccommodationStatisticsOverviewWindowViewModel : ViewModelBase
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

        private int _mostTakenYear;
        public int MostTakenYear
        {
            get
            {
                return _mostTakenYear;
            }
            set
            {
                if (_mostTakenYear != value)
                {
                    _mostTakenYear = value;
                    OnPropertyChanged(nameof(MostTakenYear));
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

        private string _selectedYear;
        public string SelectedYear
        {
            get
            {
                return _selectedYear;
            }
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged(nameof(SelectedYear));
                }

            }
        }

        public ObservableCollection<AccommodationYearStatistic> AccommodationYearStatistics { get; set; }
        public ObservableCollection<String> Years { get; set; }
        public string[] Labels { get; set; }

        private readonly AccommodationYearStatisticsService _accommodationYearStatisticsService;
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly UserService _userService;

        public SeriesCollection SeriesCollection { get; set; }
        public Func<int, string> Formatter { get; set; }
        private HashSet<int> formattedValues = new HashSet<int>();

        private User _owner;
        #endregion

        public AccommodationStatisticsOverviewWindowViewModel(Accommodation selectedAccommodation)
        {
            SelectedAccommodation = selectedAccommodation;

            _accommodationYearStatisticsService = new AccommodationYearStatisticsService();
            _accommodationReservationService = new AccommodationReservationService();
            _userService = new UserService();

            AccommodationYearStatistics = new ObservableCollection<AccommodationYearStatistic>();
            Years = new ObservableCollection<string>();

            ShowMonthlyStatisticsCommand = new RelayCommand(ShowMonthlyStatisticsCommand_Execute, ShowMonthlyStatisticsCommand_CanExecute);
            CreatePDFWithYearsCommand = new RelayCommand(CreatePDFWithYears_Execute);

            Labels = new string[0];

            _owner = _userService.GetById(selectedAccommodation.OwnerId);

            LoadColumns();
            LoadYearStatistics();
            FindMostTakenYear();
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

        private void LoadYearStatistics()
        {
            foreach(var yearStatistics in _accommodationYearStatisticsService.GetAllByAccommodationId(SelectedAccommodation.Id))
            {
                AccommodationYearStatistics.Add(yearStatistics);
                Labels = Labels.Concat(new[] { yearStatistics.Year.ToString() }).ToArray();

                SeriesCollection[0].Values.Add(yearStatistics.NumberOfReservations);
                SeriesCollection[1].Values.Add(yearStatistics.NumberOfDeclinedReservations);
                SeriesCollection[2].Values.Add(yearStatistics.NumberOfChangedReservations);
                SeriesCollection[3].Values.Add(yearStatistics.NumberOfRenovationSuggestions);

                Years.Add(yearStatistics.Year.ToString());
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

        private void FindMostTakenYear()
        {
            MostTakenYear = 0;
            int maxNumberOfTakenDays = 0;
            
            foreach (var yearStatistics in _accommodationYearStatisticsService.GetAllByAccommodationId(SelectedAccommodation.Id))
            {
                int numberOfTakenDaysInYear = _accommodationReservationService.GetNumberOfTakenDaysInYearByAccommodationId(yearStatistics.Year, yearStatistics.AccommodationId);

                if (numberOfTakenDaysInYear > maxNumberOfTakenDays)
                {
                    maxNumberOfTakenDays = numberOfTakenDaysInYear;
                    MostTakenYear = yearStatistics.Year;
                }
            }
        }

        #region COMMANDS
        public RelayCommand ShowMonthlyStatisticsCommand { get; }
        public RelayCommand CreatePDFWithYearsCommand { get; }

        public void CreatePDFWithYears_Execute(object? parameter)
        {
            AccommodationYearStatisticsPDFCreator pdfCreator = new AccommodationYearStatisticsPDFCreator(_accommodationYearStatisticsService, SelectedAccommodation);
            pdfCreator.CreatePDF(_owner);
            /*System.Diagnostics.Process.Start("explorer", "yearStatistics.pdf");*/
            string absolutePath = AppDomain.CurrentDomain.BaseDirectory + $"../../../Reports/yearStatistics.pdf";
            Process.Start(new ProcessStartInfo
            {
                FileName = absolutePath,
                UseShellExecute = true
            });
        }

        public bool ShowMonthlyStatisticsCommand_CanExecute(object? parameter)
        {
            return SelectedYear != null;
        }

        public void ShowMonthlyStatisticsCommand_Execute(object? parameter)
        {
            SelectedYearStatistics = _accommodationYearStatisticsService.FindYearStatisticsByYearAndAccommodationId(SelectedYear, SelectedAccommodation.Id);
            AccommodationMonthlyStatisticsOverview accommodationMonthlyStatisticsOverview = new AccommodationMonthlyStatisticsOverview(SelectedAccommodation, SelectedYearStatistics);
            accommodationMonthlyStatisticsOverview.Show();
        }
        #endregion
    }
}
