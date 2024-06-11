using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class AccommodationYearStatisticsService
    {
        private readonly IAccommodationYearStatisticsRepository _accommodationYearStatisticsRepository;

        public AccommodationYearStatisticsService()
        {
            _accommodationYearStatisticsRepository = Injector.CreateInstance<IAccommodationYearStatisticsRepository>();
        }

        public AccommodationYearStatistic Save(int year, Accommodation accommodation, int accommodationId, int numberOfReservations, int numberOfDeclinedReservations, int numberOfChangedReservations, int numberOfRenovationSuggestions)
        {
            return _accommodationYearStatisticsRepository.Save(year, accommodation, accommodationId, numberOfReservations, numberOfDeclinedReservations, numberOfChangedReservations, numberOfRenovationSuggestions);
        }

        public AccommodationYearStatistic FindStatisticForYearAndAccommodation(int accommodationId, int year)
        {
            return _accommodationYearStatisticsRepository.GetAll().Find(ys => ys.AccommodationId == accommodationId && ys.Year == year);
        }

        public void Update(AccommodationYearStatistic yearStatistic)
        {
            _accommodationYearStatisticsRepository.Update(yearStatistic);
        }

        public List<AccommodationYearStatistic> GetAll()
        {
            return _accommodationYearStatisticsRepository.GetAll();
        }

        public List<AccommodationYearStatistic> GetAllByAccommodationId(int accommodationId)
        {
            return GetAll().FindAll(ys => ys.AccommodationId == accommodationId);
        }

        public AccommodationYearStatistic FindYearStatisticsByYearAndAccommodationId(string selectedYear, int accommodationId)
        {
            List<AccommodationYearStatistic> allStatistics = _accommodationYearStatisticsRepository.GetAll();

            return allStatistics.Find(ys => ys.Year.ToString().Equals(selectedYear) && ys.AccommodationId == accommodationId);
        }
    }
}
