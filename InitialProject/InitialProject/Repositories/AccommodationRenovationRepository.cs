using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InitialProject.Domain.Models.AccommodationRenovation;

namespace InitialProject.Repositories
{
    public class AccommodationRenovationRepository : IAccommodationRenovationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationRenovations.csv";

        private readonly Serializer<AccommodationRenovation> _serializer;

        private List<AccommodationRenovation> _accommodationRenovations;

        public AccommodationRenovationRepository()
        {
            _serializer = new Serializer<AccommodationRenovation>();
            _accommodationRenovations = _serializer.FromCSV(FilePath);
        }

        public List<AccommodationRenovation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AccommodationRenovation Save(Accommodation accommodation, DateTime startDate, DateTime endDate, int renovationLenght, string comment)
        {
            int id = NextId();

            AccommodationRenovation accommodationRenovation = new AccommodationRenovation(id, accommodation, startDate, endDate, renovationLenght, comment);
            _accommodationRenovations.Add(accommodationRenovation);
            SaveAllRenovations();
            return accommodationRenovation;
        }

        public void SaveAllRenovations()
        {
            _serializer.ToCSV(FilePath, _accommodationRenovations);
        }

        public int NextId()
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);

            if (_accommodationRenovations.Count < 1)
            {
                return 1;
            }

            return _accommodationRenovations.Max(c => c.Id) + 1;
        }

        public AccommodationRenovation Update(AccommodationRenovation accommodationRenovation)
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            AccommodationRenovation current = _accommodationRenovations.Find(ar => ar.Id == accommodationRenovation.Id);
            int index = _accommodationRenovations.IndexOf(current);
            _accommodationRenovations.Remove(current);
            _accommodationRenovations.Insert(index, accommodationRenovation);
            _serializer.ToCSV(FilePath, _accommodationRenovations);
            return accommodationRenovation;
        }

        public void Delete(AccommodationRenovation accommodationRenovation)
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            AccommodationRenovation found = _accommodationRenovations.Find(t => t.Id == accommodationRenovation.Id);
            _accommodationRenovations.Remove(found);
            _serializer.ToCSV(FilePath, _accommodationRenovations);
        }
    }
}
