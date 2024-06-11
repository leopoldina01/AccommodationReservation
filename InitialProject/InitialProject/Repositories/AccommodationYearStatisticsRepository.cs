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
    public class AccommodationYearStatisticsRepository : IAccommodationYearStatisticsRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationYearStatistics.csv";

        private readonly Serializer<AccommodationYearStatistic> _serializer;

        private List<AccommodationYearStatistic> _accommodationYearStatistics;

        public AccommodationYearStatisticsRepository()
        {
            _serializer = new Serializer<AccommodationYearStatistic>();
            _accommodationYearStatistics = _serializer.FromCSV(FilePath);
        }

        public List<AccommodationYearStatistic> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AccommodationYearStatistic Save(int year, Accommodation accommodation, int accommodationId, int numberOfReservations, int numberOfDeclinedReservations, int numberOfChangedReservations, int numberOfRenovationSuggestions)
        {
            int id = NextId();

            AccommodationYearStatistic accommodationYearStatistic = new AccommodationYearStatistic(id, year, accommodation, accommodationId, numberOfReservations, numberOfDeclinedReservations, numberOfChangedReservations, numberOfRenovationSuggestions);
            _accommodationYearStatistics.Add(accommodationYearStatistic);
            SaveAllStatistics();
            return accommodationYearStatistic;
        }

        public void SaveAllStatistics()
        {
            _serializer.ToCSV(FilePath, _accommodationYearStatistics);
        }

        public int NextId()
        {
            _accommodationYearStatistics = _serializer.FromCSV(FilePath);

            if (_accommodationYearStatistics.Count < 1)
            {
                return 1;
            }

            return _accommodationYearStatistics.Max(c => c.Id) + 1;
        }

        public AccommodationYearStatistic Update(AccommodationYearStatistic yearStatistic)
        {
            _accommodationYearStatistics = _serializer.FromCSV(FilePath);
            AccommodationYearStatistic current = _accommodationYearStatistics.Find(t => t.Id == yearStatistic.Id);
            int index = _accommodationYearStatistics.IndexOf(current);
            _accommodationYearStatistics.Remove(current);
            _accommodationYearStatistics.Insert(index, yearStatistic);
            _serializer.ToCSV(FilePath, _accommodationYearStatistics);
            return yearStatistic;
        }
    }
}
