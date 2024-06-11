using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels
{
    public class CheckpointArrivalViewModel : ViewModelBase
    {
        #region PROPERTIES
        public ObservableCollection<User> ArrivedGuests { get; set; }
        public ObservableCollection<User> UnarrivedGuests { get; set; }

        private Checkpoint _checkpoint;
        private Tour _tour;

        private readonly TourReservationService _tourReservationService;
        private readonly CheckpointArrivalService _checkpointArrivalService;
        private readonly TourNotificationService _tourNotificationService;
        #endregion

        public CheckpointArrivalViewModel(Checkpoint checkpoint, Tour tour)
        {
            _tourReservationService = new TourReservationService();
            _checkpointArrivalService = new CheckpointArrivalService();
            _tourNotificationService = new TourNotificationService();

            _checkpoint = checkpoint;
            _tour = tour;

            ArrivedGuests = new ObservableCollection<User>();
            UnarrivedGuests = new ObservableCollection<User>();
            LoadData();

            RemoveGuestCommand = new RelayCommand(RemoveGuestCommand_Execute, RemoveGuestCommand_CanExecute);
            AddGuestCommand = new RelayCommand(AddGuestCommand_Execute, AddGuestCommand_CanExecute);
            ConfirmCommand = new RelayCommand(ConfirmCommand_Execute);
        }

        private void LoadData()
        {
            LoadArrivedGuests();
            LoadUnarrivedGuests();
        }

        private void LoadUnarrivedGuests()
        {
            UnarrivedGuests.Clear();
            foreach (var tourReservation in _tourReservationService.GetAllByTour(_tour))
            {
                if (_checkpointArrivalService.GetByReservation(tourReservation) == null)
                {
                    UnarrivedGuests.Add(tourReservation.User);
                }
            }
        }

        private void LoadArrivedGuests()
        {
            ArrivedGuests.Clear();
            foreach (var tourReservation in _tourReservationService.GetAllByTour(_tour))
            {
                if (_checkpointArrivalService.GetByReservationAndCheckpoint(tourReservation, _checkpoint) != null)
                {
                    ArrivedGuests.Add(tourReservation.User);
                }
            }
        }

        private void DeleteRemovedArrivals()
        {
            foreach (var arrival in _checkpointArrivalService.GetAllByCheckpoint(_checkpoint))
            {
                if (ArrivedGuests.FirstOrDefault(g => g.Id == arrival.Reservation.User.Id) == null)
                {
                    _checkpointArrivalService.Delete(arrival);
                }
            }
        }

        private void CreateNewArrivals()
        {
            var existingArrivals = _checkpointArrivalService.GetAllByCheckpoint(_checkpoint);
            foreach (var user in ArrivedGuests)
            {
                if (existingArrivals.FirstOrDefault(a => a.Reservation.User.Id == user.Id) == null)
                {
                    var arrival = _checkpointArrivalService.Create(_checkpoint, _tourReservationService.GetByTourIdAndUserId(_tour.Id, user.Id));
                    _tourNotificationService.Create(arrival);
                }
            }
        }

        #region COMMANDS
        public RelayCommand RemoveGuestCommand { get; }
        public RelayCommand AddGuestCommand { get; }
        public RelayCommand ConfirmCommand { get; }
        public RelayCommand CloseWindowCommand { get; }

        public void RemoveGuestCommand_Execute(object? parameter)
        {
            UnarrivedGuests.Add(parameter as User);
            ArrivedGuests.Remove(parameter as User);
        }

        public bool RemoveGuestCommand_CanExecute(object? parameter)
        {
            return parameter is not null;
        }

        public void AddGuestCommand_Execute(object? parameter)
        {
            ArrivedGuests.Add(parameter as User);
            UnarrivedGuests.Remove(parameter as User);
        }

        public bool AddGuestCommand_CanExecute(object? parameter)
        {
            return parameter is not null;
        }

        public void ConfirmCommand_Execute(object? parameter)
        {
            DeleteRemovedArrivals();

            CreateNewArrivals();
        }
        #endregion
    }
}
