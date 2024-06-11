using InitialProject.Domain.Models;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ICheckpointRepository
    {
        public List<Checkpoint> GetAll();

        public Checkpoint GetById(int id);

        public Checkpoint Save(Checkpoint checkpoint);

        public int NextId();

        public void Delete(Checkpoint checkpoint);

        public Checkpoint Update(Checkpoint checkpoint);

        public Checkpoint Create(string name, bool active, Tour tour, int tourId);
    }
}
