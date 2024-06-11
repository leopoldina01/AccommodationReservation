using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InitialProject.Domain.Models.AccommodationRenovation;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationRenovationRepository
    {
        public List<AccommodationRenovation> GetAll();
        public AccommodationRenovation Save(Accommodation accommodation, DateTime startDate, DateTime endDate, int renovationLenght, string comment);
        public void SaveAllRenovations();
        public int NextId();
        public AccommodationRenovation Update(AccommodationRenovation accommodationRenovation);
        public void Delete(AccommodationRenovation accommodationRenovation);
    }
}
