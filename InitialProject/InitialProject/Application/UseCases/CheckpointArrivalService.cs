using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class CheckpointArrivalService
    {
        private readonly ICheckpointArrivalRepository _checkpointArrivalRepository;
        private readonly TourReservationService _tourReservationService;
        private readonly CheckpointService _checkpointService;
        public CheckpointArrivalService()
        {
            _checkpointArrivalRepository = Injector.CreateInstance<ICheckpointArrivalRepository>();
            _tourReservationService = new TourReservationService();
            _checkpointService = new CheckpointService();
        }

        public IEnumerable<CheckpointArrival> GetAll()
        {
            List<CheckpointArrival> arrivals = new();
            foreach (var arrival in _checkpointArrivalRepository.GetAll())
            {
                arrival.Reservation = _tourReservationService.GetById(arrival.ReservationId);
                arrival.Checkpoint = _checkpointService.GetById(arrival.CheckpointId);
                arrivals.Add(arrival);
            }
            return arrivals;
        }

        public IEnumerable<CheckpointArrival> GetAllByTour(Tour tour)
        {
            List<CheckpointArrival> arrivals = new();
            foreach (var arrival in _checkpointArrivalRepository.GetAll())
            {
                arrival.Reservation = _tourReservationService.GetById(arrival.ReservationId);
                arrival.Checkpoint = _checkpointService.GetById(arrival.CheckpointId);
                if (arrival.Checkpoint.TourId == tour.Id)
                {
                    arrivals.Add(arrival);
                }
            }
            return arrivals;
        }

        public CheckpointArrival GetById(int id)
        {
            var arrival = _checkpointArrivalRepository.GetById(id);
            arrival.Reservation = _tourReservationService.GetById(arrival.ReservationId);
            arrival.Checkpoint = _checkpointService.GetById(arrival.CheckpointId);
            return arrival;
        }

        public void Remove(int id)
        {
            _checkpointArrivalRepository.RemoveById(id);
        }


        public CheckpointArrival GetByReservation(TourReservation tourReservation)
        {
            var arrival = _checkpointArrivalRepository.GetAll().FirstOrDefault(a => a.ReservationId == tourReservation.Id);
            if (arrival is null) return null;
            arrival.Reservation = tourReservation;
            arrival.Checkpoint = _checkpointService.GetById(arrival.CheckpointId);
            return arrival;
        }

        public CheckpointArrival GetByReservationAndCheckpoint(TourReservation tourReservation, Checkpoint checkpoint)
        {
            var arrival = _checkpointArrivalRepository.GetAll().FirstOrDefault(a => a.ReservationId == tourReservation.Id && a.CheckpointId == checkpoint.Id);
            if (arrival is null) return null;
            arrival.Reservation = tourReservation;
            arrival.Checkpoint = checkpoint;
            return arrival;
        }

        public IEnumerable<CheckpointArrival> GetAllByCheckpoint(Checkpoint checkpoint)
        {
            var arrivals = _checkpointArrivalRepository.GetAll().Where(a => a.CheckpointId == checkpoint.Id);
            foreach (var arrival in arrivals)
            {
                arrival.Reservation = _tourReservationService.GetById(arrival.ReservationId);
                arrival.Checkpoint = checkpoint;
            }
            return arrivals;
        }

        public void Delete(CheckpointArrival arrival)
        {
            _checkpointArrivalRepository.Delete(arrival);
        }

        public CheckpointArrival Create(Checkpoint checkpoint, TourReservation reservation)
        {
            var arrival = _checkpointArrivalRepository.Create(checkpoint.Id, reservation.Id);
            arrival.Reservation = _tourReservationService.GetById(arrival.ReservationId);
            arrival.Checkpoint = _checkpointService.GetById(arrival.CheckpointId);
            return arrival;
        }
    }
}
