using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Models;
using InitialProject.Repositories;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationRenovationSuggestionRepository
    {
        public List<AccommodationRenovationSuggestion> GetAll();
        public AccommodationRenovationSuggestion Save(string comment, int levelOfUrgency, int reservationId, int ownerId, int suggesterId);
        public int NextId();
    }
}
