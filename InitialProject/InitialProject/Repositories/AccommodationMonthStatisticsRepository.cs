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
    public class AccommodationMonthStatisticsRepository : IAccommodationMonthStatisticsRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationMonthStatistics.csv";

        private readonly Serializer<AccommodationMonthStatistics> _serializer;

        private List<AccommodationMonthStatistics> _accommodationMonthStatistics;

        public AccommodationMonthStatisticsRepository()
        {
            _serializer = new Serializer<AccommodationMonthStatistics>();
            _accommodationMonthStatistics = _serializer.FromCSV(FilePath);
        }

        public List<AccommodationMonthStatistics> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AccommodationMonthStatistics Save(int month, AccommodationYearStatistic yearStatistic, int yearStatisticId, int numberOfReservations, int numberOfDeclinedReservations, int numberOfChangedReservations, int numberOfRenovationSuggestions)
        {
            int id = NextId();

            AccommodationMonthStatistics accommodationMonthStatistic = new AccommodationMonthStatistics(id, month, yearStatistic, yearStatisticId, numberOfReservations, numberOfDeclinedReservations, numberOfChangedReservations, numberOfRenovationSuggestions);
            _accommodationMonthStatistics.Add(accommodationMonthStatistic);
            SaveAllStatistics();
            return accommodationMonthStatistic;
        }

        public void SaveAllStatistics()
        {
            _serializer.ToCSV(FilePath, _accommodationMonthStatistics);
        }

        public int NextId()
        {
            _accommodationMonthStatistics = _serializer.FromCSV(FilePath);

            if (_accommodationMonthStatistics.Count < 1)
            {
                return 1;
            }

            return _accommodationMonthStatistics.Max(c => c.Id) + 1;
        }

        public AccommodationMonthStatistics Update(AccommodationMonthStatistics monthStatistic)
        {
            _accommodationMonthStatistics = _serializer.FromCSV(FilePath);
            AccommodationMonthStatistics current = _accommodationMonthStatistics.Find(t => t.Id == monthStatistic.Id);
            int index = _accommodationMonthStatistics.IndexOf(current);
            _accommodationMonthStatistics.Remove(current);
            _accommodationMonthStatistics.Insert(index, monthStatistic);
            _serializer.ToCSV(FilePath, _accommodationMonthStatistics);
            return monthStatistic;
        }
    }
}
