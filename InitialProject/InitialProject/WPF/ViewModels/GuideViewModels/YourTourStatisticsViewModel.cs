using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModels
{
    public class YourTourStatisticsViewModel : ViewModelBase
    {
        #region PROPERTIES
        private Tour? _selectedTour;
        public Tour? SelectedTour
        {
            get
            {
                return _selectedTour;
            }
            set
            {
                if (_selectedTour != value)
                {
                    _selectedTour = value;
                    OnPropertyChanged(nameof(SelectedTour));
                }
            }
        }

        public ObservableCollection<Tour> PastTours { get; set; }

        private readonly TourService _tourService;

        private readonly User _guide;
        #endregion
        public YourTourStatisticsViewModel(User guide)
        {
            _guide = guide;

            _tourService = new TourService();

            PastTours = new ObservableCollection<Tour>(_tourService.GetPastToursByGuide(_guide));

            OpenStatsCommand = new RelayCommand(OpenStatsCommand_Execute, OpenStatsCommand_CanExecute);
            MostVisitedTourCommand = new RelayCommand(MostVisitedTourCommand_Execute);
        }

        #region COMMANDS
        public RelayCommand OpenStatsCommand { get; }
        public RelayCommand MostVisitedTourCommand { get; }

        public void MostVisitedTourCommand_Execute(object? parameter)
        {
            var mostVisitedTourView = new MostVisitedTourView();
            mostVisitedTourView.Show();
        }

        public void OpenStatsCommand_Execute(object? parameter)
        {
            var tourStatisticsView = new TourStatisticsView(parameter as Tour);
            tourStatisticsView.Show();
        }

        public bool OpenStatsCommand_CanExecute(object? parameter)
        {
            return parameter is not null;
        }
        #endregion
    }
}
