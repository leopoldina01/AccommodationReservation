using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class CheckpointService
    {
        private readonly ICheckpointRepository _checkpointRepository;
        private readonly TourService _tourService;
        public CheckpointService()
        {
            _checkpointRepository = Injector.CreateInstance<ICheckpointRepository>();
            _tourService = new TourService();
        }

        public Checkpoint GetById(int id)
        {
            var checkpoint = _checkpointRepository.GetAll().FirstOrDefault(c => c.Id == id);
            checkpoint.Tour = _tourService.GetById(checkpoint.TourId);
            return checkpoint;
        }

        public IEnumerable<Checkpoint> GetAllByTour(Tour tour)
        {
            var checkpoints = _checkpointRepository.GetAll().Where(c => c.TourId == tour.Id).ToList();
            checkpoints.ForEach(c => c.Tour = tour);
            return checkpoints;
        }

        public void ActivateCheckpoint(Checkpoint checkpoint)
        {
            checkpoint.Active = true;
            _checkpointRepository.Update(checkpoint);
        }

        public Checkpoint Create(string name, Tour tour)
        {
            return _checkpointRepository.Create(name, false, tour, tour.Id);
        }
    }
}
