using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationYearStatisticsRepository
    {
        public List<AccommodationYearStatistic> GetAll();
        public AccommodationYearStatistic Save(int year, Accommodation accommodation, int accommodationId, int numberOfReservations, int numberOfDeclinedReservations, int numberOfChangedReservations, int numberOfRenovationSuggestions);
        public void SaveAllStatistics();
        public int NextId();
        public AccommodationYearStatistic Update(AccommodationYearStatistic yearStatistic);
    }
}
