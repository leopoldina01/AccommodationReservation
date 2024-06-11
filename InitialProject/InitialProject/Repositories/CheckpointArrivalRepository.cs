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
    public class CheckpointArrivalRepository : ICheckpointArrivalRepository
    {
        private const string FilePath = "../../../Resources/Data/checkpointArrivals.csv";

        private readonly Serializer<CheckpointArrival> _serializer;

        private List<CheckpointArrival> _checkpointArrivals;

        public CheckpointArrivalRepository()
        {
            _serializer = new Serializer<CheckpointArrival>();
            _checkpointArrivals = _serializer.FromCSV(FilePath);
        }

        public List<CheckpointArrival> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public CheckpointArrival Save(CheckpointArrival checkpointArrival)
        {
            checkpointArrival.Id = NextId();
            _checkpointArrivals = _serializer.FromCSV(FilePath);
            _checkpointArrivals.Add(checkpointArrival);
            _serializer.ToCSV(FilePath, _checkpointArrivals);
            return checkpointArrival;
        }

        public int NextId()
        {
            _checkpointArrivals = _serializer.FromCSV(FilePath);
            if (_checkpointArrivals.Count < 1)
            {
                return 1;
            }

            return _checkpointArrivals.Max(c => c.Id) + 1;
        }

        public CheckpointArrival GetById(int id)
        {
            _checkpointArrivals = _serializer.FromCSV(FilePath);
            return _checkpointArrivals.FirstOrDefault(c => c.Id == id);  
        }

        public CheckpointArrival GetByReservationId(int id)
        {
            _checkpointArrivals = _serializer.FromCSV(FilePath);
            return _checkpointArrivals.FirstOrDefault(c => c.ReservationId == id);
        }

        public IEnumerable<CheckpointArrival> GetAllByCheckpointId(int checkpointId)
        {
            _checkpointArrivals = _serializer.FromCSV(FilePath);
            return _checkpointArrivals.Where(c => c.CheckpointId == checkpointId);
        }

        public void Delete(CheckpointArrival checkpointArrival)
        {
            _checkpointArrivals = _serializer.FromCSV(FilePath);
            CheckpointArrival found = _checkpointArrivals.Find(c => c.Id == checkpointArrival.Id);
            _checkpointArrivals.Remove(found);
            _serializer.ToCSV(FilePath, _checkpointArrivals);
        }

        public CheckpointArrival Create(int checkpointId, int reservationId)
        {
            _checkpointArrivals = _serializer.FromCSV(FilePath);
            CheckpointArrival newCheckpointArrival = new CheckpointArrival(NextId(), checkpointId, reservationId);
            _checkpointArrivals.Add(newCheckpointArrival);
            _serializer.ToCSV(FilePath, _checkpointArrivals);
            return newCheckpointArrival;
        }

        public void RemoveById(int checkpointId)
        {
            _checkpointArrivals = _serializer.FromCSV(FilePath);
            CheckpointArrival current = _checkpointArrivals.Find(c => c.Id == checkpointId);
            _checkpointArrivals.Remove(current);
            _serializer.ToCSV(FilePath, _checkpointArrivals);
        }
    }
}
