using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationMonthStatisticsRepository
    {
        public List<AccommodationMonthStatistics> GetAll();
        public AccommodationMonthStatistics Save(int month, AccommodationYearStatistic yearStatistic, int yearStatisticId, int numberOfReservations, int numberOfDeclinedReservations, int numberOfChangedReservations, int numberOfRenovationSuggestions);
        public void SaveAllStatistics();
        public int NextId();
        public AccommodationMonthStatistics Update(AccommodationMonthStatistics monthStatistic);
    }
}
