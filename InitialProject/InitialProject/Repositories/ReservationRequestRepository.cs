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
    public class ReservationRequestRepository : IReservationRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/reservationRequests.csv";

        private readonly Serializer<ReservationRequest> _serializer;

        private List<ReservationRequest> _requests;

        public ReservationRequestRepository()
        {
            _serializer = new Serializer<ReservationRequest>();
            _requests = _serializer.FromCSV(FilePath);
        }

        public List<ReservationRequest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public ReservationRequest Save(DateTime newStartDate, DateTime newEndDate, RequestStatus status, AccommodationReservation reservation)
        {
            int id = NextId();

            ReservationRequest request = new ReservationRequest(id, newStartDate, newEndDate, status, reservation);
            _requests.Add(request);
            SaveAllRequests();
            return request;
        }

        public void SaveAllRequests()
        {
            _serializer.ToCSV(FilePath, _requests);
        }

        public int NextId()
        {
            _requests = _serializer.FromCSV(FilePath);

            if (_requests.Count < 1)
            {
                return 1;
            }

            return _requests.Max(c => c.Id) + 1;
        }

        public void Delete(ReservationRequest request)
        {
            _requests = _serializer.FromCSV(FilePath);
            ReservationRequest found = _requests.Find(t => t.Id == request.Id);
            _requests.Remove(found);
            _serializer.ToCSV(FilePath, _requests);
        }

        public ReservationRequest Update(ReservationRequest request)
        {
            _requests = _serializer.FromCSV(FilePath);
            ReservationRequest current = _requests.Find(t => t.Id == request.Id);
            int index = _requests.IndexOf(current);
            _requests.Remove(current);
            _requests.Insert(index, request);
            _serializer.ToCSV(FilePath, _requests);
            return request;
        }
    }
}
