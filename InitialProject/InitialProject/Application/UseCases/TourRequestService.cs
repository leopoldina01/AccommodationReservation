using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class TourRequestService
    {
        private readonly ITourRequestRepository _tourRequestRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly LocationService _locationService;
        private readonly UserService _userService;

        public TourRequestService()
        {
            _tourRequestRepository = Injector.CreateInstance<ITourRequestRepository>();
            _locationRepository = Injector.CreateInstance<ILocationRepository>();
            _locationService = new LocationService();
            _userService = new UserService();
        }

        public IEnumerable<TourRequest> GetAll()
        {
            var requests = _tourRequestRepository.GetAll();
            foreach (var request in requests)
            {
                request.Location = _locationService.GetLocationById(request.LocationId);
                request.Guide = _userService.GetById(request.GuideId);
            }
            return requests;
        }

        public Location FillLocation(string country, string city)
        {
            return _locationRepository.GetByCountryAndCity(country, city);
        }

        public TourRequest Save(TourRequest tourRequest)
        {
            return _tourRequestRepository.Save(tourRequest);
        }

        public void Update(TourRequest tourRequest)
        {
            _tourRequestRepository.Update(tourRequest);
        }

        public Location GetMostWantedLocation()
        {
            var locationId = _tourRequestRepository.GetAll().GroupBy(r => r.LocationId).OrderByDescending(r => r.Count()).Select(r => r.Key).FirstOrDefault();
            return _locationService.GetLocationById(locationId);
        }

        public string GetMostWantedLanguage()
        {
            return _tourRequestRepository.GetAll().GroupBy(r => r.Language).OrderByDescending(r => r.Count()).Select(r => r.Key).FirstOrDefault();
        }

        public IEnumerable<TourRequest> GetAllByGuide(User guide)
        {
            var requests = _tourRequestRepository.GetAll().Where(r => r.GuideId == guide.Id);
            foreach (var request in requests)
            {
                request.Location = _locationService.GetLocationById(request.LocationId);
                request.Guide = _userService.GetById(request.GuideId);
            }
            return requests;
        }

        public void AcceptRequest(TourRequest request)
        {
            request.Status = TourRequestStatus.ACCEPTED;
            _tourRequestRepository.Update(request);
        }

        public IEnumerable<Location> GetRequestedLocations()
        {
            List<Location> locations = new List<Location>();
            foreach(var req in GetAll())
            {
                locations.Add(req.Location);
            }
            return locations;
        }

        public TourRequest GetById(int id)
        {
            var request = new TourRequest();
            request = _tourRequestRepository.GetById(id);

            request.Guide = _userService.GetById(request.GuideId);
            request.Location = _locationService.GetLocationById(request.LocationId);
            return request; 
        }
    }
}
