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
    public class AccommodationRenovationSuggestionRepository : IAccommodationRenovationSuggestionRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationRenovationSuggestions.csv";

        private readonly Serializer<AccommodationRenovationSuggestion> _serializer;

        private List<AccommodationRenovationSuggestion> _accommodationRenovationSuggestions;

        public AccommodationRenovationSuggestionRepository()
        {
            _serializer = new Serializer<AccommodationRenovationSuggestion>();
            _accommodationRenovationSuggestions = _serializer.FromCSV(FilePath);
        }

        public List<AccommodationRenovationSuggestion> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AccommodationRenovationSuggestion Save(string comment, int levelOfUrgency, int reservationId, int ownerId, int suggesterId)
        {
            int id = NextId();

            AccommodationRenovationSuggestion accommodationRenovationSuggestion = new AccommodationRenovationSuggestion(id, comment, levelOfUrgency, reservationId, ownerId, suggesterId);

            _accommodationRenovationSuggestions.Add(accommodationRenovationSuggestion);
            _serializer.ToCSV(FilePath, _accommodationRenovationSuggestions);
            return accommodationRenovationSuggestion;
        }

        public int NextId()
        {
            _accommodationRenovationSuggestions = _serializer.FromCSV(FilePath);
            if (_accommodationRenovationSuggestions.Count < 1)
            {
                return 1;
            }

            return _accommodationRenovationSuggestions.Max(c => c.Id) + 1;
        }
    }
}
