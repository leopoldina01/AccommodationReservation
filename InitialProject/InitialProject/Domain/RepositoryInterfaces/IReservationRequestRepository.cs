using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IReservationRequestRepository
    {
        public List<ReservationRequest> GetAll();
        public ReservationRequest Save(DateTime newStartDate, DateTime newEndDate, RequestStatus status, AccommodationReservation reservation);
        public void SaveAllRequests();
        public int NextId();
        public ReservationRequest Update(ReservationRequest request);
        public void Delete(ReservationRequest request);
    }
}
