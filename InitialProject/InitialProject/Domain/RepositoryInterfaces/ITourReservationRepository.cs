using InitialProject.Domain.Models;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourReservationRepository
    {
        public List<TourReservation> GetAll();
        public TourReservation Save(int tourId, int userId, int? numberOfPeople, float age);
        public int NextId();
        public TourReservation GetById(int id);
        public void Delete(TourReservation reservation);
    }
}
