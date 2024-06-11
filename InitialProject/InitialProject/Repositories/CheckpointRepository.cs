using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class CheckpointRepository : ICheckpointRepository
    {
        private const string FilePath = "../../../Resources/Data/checkpoints.csv";

        private readonly Serializer<Checkpoint> _serializer;

        private List<Checkpoint> _checkpoints;

        public CheckpointRepository()
        {
            _serializer = new Serializer<Checkpoint>();
            _checkpoints = _serializer.FromCSV(FilePath);
        }

        public List<Checkpoint> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Checkpoint GetById(int id)
        {
            _checkpoints = _serializer.FromCSV(FilePath);
            return _checkpoints.FirstOrDefault(c => c.Id == id);
        }

        public Checkpoint Save(Checkpoint checkpoint)
        {
            checkpoint.Id = NextId();
            _checkpoints = _serializer.FromCSV(FilePath);
            _checkpoints.Add(checkpoint);
            _serializer.ToCSV(FilePath, _checkpoints);
            return checkpoint;
        }

        public int NextId()
        {
            _checkpoints = _serializer.FromCSV(FilePath);
            if (_checkpoints.Count < 1)
            {
                return 1;
            }
            return _checkpoints.Max(p => p.Id) + 1;
        }

        public void Delete(Checkpoint checkpoint)
        {
            _checkpoints = _serializer.FromCSV(FilePath);
            Checkpoint found = _checkpoints.Find(p => p.Id == checkpoint.Id);
            _checkpoints.Remove(found);
            _serializer.ToCSV(FilePath, _checkpoints);
        }

        public Checkpoint Update(Checkpoint checkpoint)
        {
            _checkpoints = _serializer.FromCSV(FilePath);
            Checkpoint current = _checkpoints.Find(p => p.Id == checkpoint.Id);
            int index = _checkpoints.IndexOf(current);
            _checkpoints.Remove(current);
            _checkpoints.Insert(index, checkpoint);
            _serializer.ToCSV(FilePath, _checkpoints);
            return checkpoint;
        }

        public Checkpoint Create(string name, bool active, Tour tour, int tourId)
        {
            _checkpoints = _serializer.FromCSV(FilePath);
            Checkpoint newCheckpoint = new Checkpoint(NextId(), name, active, tour, tourId);
            _checkpoints.Add(newCheckpoint);
            _serializer.ToCSV(FilePath, _checkpoints);
            return newCheckpoint;
        }
    }
}
