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
    public class TourRepository : ITourRepository
    {
        private const string FilePath = "../../../Resources/Data/tours.csv";

        private readonly Serializer<Tour> _serializer;

        private List<Tour> _tours;

        public TourRepository()
        {
            _serializer = new Serializer<Tour>();
            _tours = _serializer.FromCSV(FilePath);
        }

        public List<Tour> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Tour Save(Tour tour)
        {
            tour.Id = NextId();
            _tours = _serializer.FromCSV(FilePath);
            _tours.Add(tour);
            _serializer.ToCSV(FilePath, _tours);
            return tour;
        }

        public int NextId()
        {
            _tours = _serializer.FromCSV(FilePath);
            if (_tours.Count < 1)
            {
                return 1;
            }
            return _tours.Max(t => t.Id) + 1;
        }

        public void Delete(Tour tour)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour found = _tours.Find(t => t.Id == tour.Id);
            _tours.Remove(found);
            _serializer.ToCSV(FilePath, _tours);
        }

        public Tour Update(Tour tour)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour current = _tours.Find(t => t.Id == tour.Id);
            int index = _tours.IndexOf(current);
            _tours.Remove(current);
            _tours.Insert(index, tour);
            _serializer.ToCSV(FilePath, _tours);
            return tour;
        }

        public Tour Create(string name, Location location, int locationId, string description, string language, int maxGuests, DateTime startTime, double duration, string coverImageUrl, int guideId)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour newTour = new Tour(NextId(), name, location, locationId, description, language, maxGuests, startTime, duration, coverImageUrl, guideId, TourStatus.NOT_STARTED);
            _tours.Add(newTour);
            _serializer.ToCSV(FilePath, _tours);
            return newTour;
        }

        public Tour GetById(int id)
        {
            _tours = _serializer.FromCSV(FilePath);
            foreach(var tour in _tours)
            {
                if (tour.Id == id)
                {
                    return tour;
                }
            }
            return null;
        }
    }
}
