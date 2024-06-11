using InitialProject.Domain.Models;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ICheckpointArrivalRepository
    {
        public List<CheckpointArrival> GetAll();
        public CheckpointArrival Save(CheckpointArrival checkpointArrival);
        public int NextId();
        public CheckpointArrival GetById(int id);
        public CheckpointArrival GetByReservationId(int id);
        public IEnumerable<CheckpointArrival> GetAllByCheckpointId(int checkpointId);
        public void Delete(CheckpointArrival checkpointArrival);
        public CheckpointArrival Create(int checkpointId, int userId);
        public void RemoveById(int checkpointId);
    }
}
