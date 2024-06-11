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
    public class TourReservationRepository : ITourReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/tourReservations.csv";

        private readonly Serializer<TourReservation> _serializer;

        private List<TourReservation> _tourReservations;

        public TourReservationRepository()
        {
            _serializer = new Serializer<TourReservation>();
            _tourReservations = _serializer.FromCSV(FilePath);
        }

        public List<TourReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public TourReservation GetById(int id)
        {
            _tourReservations = _serializer.FromCSV(FilePath);
            return _tourReservations.FirstOrDefault(t =>  t.Id == id);
        }

        public TourReservation Save(int tourId, int userId, int? numberOfPeople, float age)
        {
            int id = NextId();

            TourReservation tourReservation = new TourReservation(id, tourId, userId, numberOfPeople, age);

            _tourReservations.Add(tourReservation);
            _serializer.ToCSV(FilePath, _tourReservations);
            return tourReservation;
        }

        public int NextId()
        {
            _tourReservations = _serializer.FromCSV(FilePath);
            if (_tourReservations.Count < 1)
            {
                return 1;
            }

            return _tourReservations.Max(c => c.Id) + 1;
        }

        public void Delete(TourReservation reservation)
        {
            _tourReservations = _serializer.FromCSV(FilePath);
            TourReservation found = _tourReservations.Find(t => t.Id == reservation.Id);
            _tourReservations.Remove(found);
            _serializer.ToCSV(FilePath, _tourReservations);
        }
    }
}
