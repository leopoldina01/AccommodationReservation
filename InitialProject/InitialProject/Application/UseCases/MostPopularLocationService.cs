using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class MostPopularLocationService
    {
        private readonly AccommodationService _accommodationService;
        private readonly AccommodationMonthStatisticsService _accommodationMonthStatisticsService;
        private readonly LocationService _locationService;
        private readonly AccommodationYearStatisticsService _accommodationYearStatisticsService;

        public MostPopularLocationService()
        {
            _accommodationService = new AccommodationService();
            _accommodationMonthStatisticsService = new AccommodationMonthStatisticsService();
            _locationService = new LocationService();
            _accommodationYearStatisticsService = new AccommodationYearStatisticsService();
        }

        public Dictionary<Location, int> FindAllUsedLocations(int ownerId = -1)
        {
            List<Accommodation> accommodations = FilteredAccommodations(ownerId);

            Dictionary<Location, int> locations = new Dictionary<Location, int>();

            accommodations = FillInLocation(accommodations);

            foreach (var accommodation in accommodations)
            {
                locations.Add(accommodation.Location, 0);
            }

            return locations;
        }

        private List<Accommodation> FilteredAccommodations(int ownerId)
        {
            if (ownerId == -1)
            {
                return _accommodationService.GetAll();
            }

            return _accommodationService.GetAll().FindAll(a => a.OwnerId == ownerId);
        }

        private List<Accommodation> FillInLocation(List<Accommodation> accommodations)
        {
            List<Accommodation> updatedAccommodations = new List<Accommodation>();

            foreach (var accommodation in accommodations)
            {
                accommodation.Location = _locationService.GetLocationById(accommodation.LocationId);
                updatedAccommodations.Add(accommodation);
            }

            return updatedAccommodations;
        }

        public Dictionary<Location, int> CalculateAllReservations(Dictionary<Location, int> locations)
        {
            Dictionary<Location, int> updatedLocations = locations;

            foreach (var location in locations)
            {
                List<Accommodation> accommodations = _accommodationService.GetAll().FindAll(a => a.LocationId == location.Key.Id);

                int numberOfReservations = 0;

                foreach (Accommodation accommodation in accommodations)
                {
                    List<AccommodationMonthStatistics> monthStatistics = _accommodationMonthStatisticsService.GetAll();

                    monthStatistics = FillInYear(monthStatistics);

                    monthStatistics = monthStatistics.FindAll(ams => ams.YearStatistics.Year == DateTime.Now.Year && ams.YearStatistics.AccommodationId == accommodation.Id);

                    foreach (var statistic in monthStatistics)
                    {
                        numberOfReservations += statistic.NumberOfReservations;
                    }
                }

                updatedLocations[location.Key] = numberOfReservations;
            }

            return updatedLocations;
        }

        private List<AccommodationMonthStatistics> FillInYear(List<AccommodationMonthStatistics> monthStatistics)
        {
            List<AccommodationMonthStatistics> updatedMonthStatistics = new List<AccommodationMonthStatistics>();

            foreach (var month in monthStatistics)
            {
                month.YearStatistics = _accommodationYearStatisticsService.GetAll().Find(ys => ys.Id == month.YearStatisticsId);
                updatedMonthStatistics.Add(month);
            }

            return updatedMonthStatistics;
        }

        public Location FindMostPopular()
        {
            Dictionary<Location, int> locations = FindAllUsedLocations();

            locations = CalculateAllReservations(locations);

            Location mostPopularLocation = locations.FirstOrDefault(x => x.Value == locations.Values.Max()).Key;

            return mostPopularLocation;
        }

        public Location FindLeastPopular(int ownerId)
        {
            Dictionary<Location, int> locations = FindAllUsedLocations(ownerId);

            locations = CalculateAllReservations(locations);

            Location mostPopularLocation = locations.FirstOrDefault(x => x.Value == locations.Values.Min()).Key;

            return mostPopularLocation;
        }
    }
}
