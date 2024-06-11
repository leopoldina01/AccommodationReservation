using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.Repositories;
using InitialProject.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels
{
    public class TodaysToursViewModel : ViewModelBase
    {
        #region PROPERTIES
        private Tour? _activeTour;
        public Tour? ActiveTour
        {
            get => _activeTour;
            set
            {
                if (_activeTour != value)
                {
                    _activeTour = value;
                    OnPropertyChanged(nameof(ActiveTour));
                }
            }
        }

        public ObservableCollection<Tour> TodaysTours { get; set; }
        public ObservableCollection<Checkpoint> Checkpoints { get; set; }

        private readonly User _guide;

        private readonly TourService _tourService;
        #endregion

        public TodaysToursViewModel(User guide)
        {
            _tourService = new TourService();

            _guide = guide;

            TodaysTours = new ObservableCollection<Tour>();
            Checkpoints = new ObservableCollection<Checkpoint>();

            LoadTodaysTours();

            ActivateOrFinishTourCommand = new RelayCommand(ActivateOrFinishTourCommand_Execute, ActivateOrFinishTourCommand_CanExecute);
            CheckpointsCommand = new RelayCommand(CheckpointsCommand_Execute, CheckpointsCommand_CanExecute);
        }

        public void LoadTodaysTours()
        {
            TodaysTours.Clear();
            foreach (var tour in _tourService.GetTodaysToursByGuideId(_guide.Id))
            {
                TodaysTours.Add(tour);
                if (_tourService.IsTourActive(tour))
                {
                    ActiveTour = tour;
                }
            }
        }

        #region COMMANDS
        public RelayCommand ActivateOrFinishTourCommand { get; }
        public RelayCommand CheckpointsCommand { get; }

        public void ActivateOrFinishTourCommand_Execute(object? parameter)
        {
            if(ActiveTour is null)
            {
                _tourService.ActivateTour(parameter as Tour);
                LoadTodaysTours();
            }
            else
            {
                _tourService.FinishTour(ActiveTour);
                ActiveTour = null;
                LoadTodaysTours();
            }
        }

        public bool ActivateOrFinishTourCommand_CanExecute(object? parameter)
        {
            Tour? tour = parameter as Tour;
            if (ActiveTour is null)
            {
                return tour is not null && tour.Status == TourStatus.NOT_STARTED;
            }
            else
            {
                return tour is not null && tour.Status == TourStatus.ACTIVE;
            }
        }

        public void CheckpointsCommand_Execute(object? parameter)
        {
            var tourCheckpointsView = new TourCheckpointsView(parameter as Tour, this);
            tourCheckpointsView.Show();
        }

        public bool CheckpointsCommand_CanExecute(object? parameter)
        {
            Tour? tour = parameter as Tour;
            return tour is not null && tour.Status == TourStatus.ACTIVE;
        }
        #endregion
    }
}
