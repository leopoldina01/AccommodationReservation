using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class AccommodationRenovationSuggestionService
    {
        private readonly IAccommodationRenovationSuggestionRepository _accommodationRenovationSuggestionRepository;

        public AccommodationRenovationSuggestionService()
        {
            _accommodationRenovationSuggestionRepository = Injector.CreateInstance<IAccommodationRenovationSuggestionRepository>();
        }

        public AccommodationRenovationSuggestion SaveSuggestion(string comment, int levelOfUrgency, int reservationId, int ownerId, int suggesterId)
        {
            return _accommodationRenovationSuggestionRepository.Save(comment, levelOfUrgency, reservationId, ownerId, suggesterId);
        }
    }
}
