using InitialProject.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class TourRequestStatisticsService
    {
        public TourRequestService _tourRequestService;
        public LocationService _locationService;
        
        public TourRequestStatisticsService()
        {
            _tourRequestService = new TourRequestService();
            _locationService = new LocationService();
        }

        public IEnumerable<int> GetAllYearsWithRequests()
        {
            return _tourRequestService.GetAll().Select(r => r.RequestArrivalDate.Year).ToHashSet();
        }

        public int GetNumberOfRequestsForYear(int year)
        {
            return _tourRequestService.GetAll().Where(r => r.RequestArrivalDate.Year == year).Count();
        }

        public int GetNumberOfRequestsForMonth(int month, int year)
        {
            return _tourRequestService.GetAll().Where(r => r.RequestArrivalDate.Year == year && r.RequestArrivalDate.Month == month).Count();
        }

        public IEnumerable<int> FilterYearsWithRequests(string country, string city, string language)
        {
            return _tourRequestService.GetAll().Where(r => r.Location.Country == country && r.Location.City == city && r.Language == language).Select(r => r.RequestArrivalDate.Year).ToHashSet();
        }

        public int FilterNumberOfRequestsForYear(int year, string country, string city, string language)
        {
            return _tourRequestService.GetAll().Where(r => r.RequestArrivalDate.Year == year && r.Location.Country == country && r.Location.City == city && r.Language == language).Count();
        }

        public int FilterNumberOfRequestsForMonth(int month, int year, string country, string city, string language)
        {
            return _tourRequestService.GetAll().Where(r => r.RequestArrivalDate.Year == year && r.RequestArrivalDate.Month == month && r.Location.Country == country && r.Location.City == city && r.Language == language).Count();
        }

        public IEnumerable<LocationStatistics> GetLocationStats()
        {
            List<LocationStatistics> stats = new List<LocationStatistics>();
            int counter = 0;
            foreach (var location in _tourRequestService.GetRequestedLocations())
            {
                counter = 0;
                foreach (var req in _tourRequestService.GetAll())
                {
                    if (location.Id == req.LocationId)
                    {
                        counter++;
                    }
                }
                if (counter != 0)
                {
                    LocationStatistics locationStatistics = new LocationStatistics(location, counter);
                    stats.Add(locationStatistics);
                }
            }
            return stats;
        }


    }
}
