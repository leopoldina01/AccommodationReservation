using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourRequestRepository
    {
        public IEnumerable<TourRequest> GetAll();
        public TourRequest Save(TourRequest tourRequest);
        public int NextId();
        public void Delete(TourRequest tourRequest);
        public TourRequest Update(TourRequest tourRequest);
        public TourRequest GetById(int id);
    }
}
