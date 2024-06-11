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

namespace InitialProject.WPF.ViewModels
{
    public class TourCheckpointsViewModel : ViewModelBase
    {
        #region PROPERTIES
        public ObservableCollection<Checkpoint> Checkpoints { get; set; }

        private readonly Tour _tour;
        private readonly CheckpointService _checkpointService;
        private readonly TourService _tourService;
        private readonly TodaysToursViewModel _todaysToursViewModel;
        #endregion
        public TourCheckpointsViewModel(Tour tour, TodaysToursViewModel todaysToursViewModel)
        {
            _checkpointService = new CheckpointService();
            _tourService = new TourService();

            _tour = tour;
            _todaysToursViewModel = todaysToursViewModel;

            Checkpoints = new ObservableCollection<Checkpoint>();

            LoadCheckpoints();

            CompleteCheckpointCommand = new RelayCommand(CompleteCheckpointCommand_Execute, CompleteCheckpointCommand_CanExecute);
            GuestListCommand = new RelayCommand(GuestListCommand_Execute);
        }

        private void LoadCheckpoints()
        {
            Checkpoints.Clear();
            foreach (var checkpoint in _checkpointService.GetAllByTour(_tour))
            {
                Checkpoints.Add(checkpoint);
            }
        }

        private void UpdateParent()
        {
            _todaysToursViewModel.LoadTodaysTours();
            _todaysToursViewModel.ActiveTour = null;
        }

        #region COMMANDS
        public RelayCommand CompleteCheckpointCommand { get; }
        public RelayCommand GuestListCommand { get; }
        public RelayCommand CloseWindowCommand { get; }

        public void CompleteCheckpointCommand_Execute(object? parameter)
        {
            Checkpoint checkpoint = parameter as Checkpoint;
            var isLastCheckpoint = Checkpoints.IndexOf(checkpoint) + 1 == Checkpoints.Count;

            _checkpointService.ActivateCheckpoint(checkpoint);

            if (isLastCheckpoint)
            {
                _tourService.FinishTour(_tour);
                UpdateParent();
            }

            LoadCheckpoints();
        }

        public bool CompleteCheckpointCommand_CanExecute(object? parameter)
        {
            return parameter is not null;
        }

        public void GuestListCommand_Execute(object? parameter)
        {
            var checkpointArrivalView = new CheckpointArrivalView(parameter as Checkpoint, _tour);
            checkpointArrivalView.Show();
        }
        #endregion
    }
}
