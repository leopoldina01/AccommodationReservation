using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class LocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService()
        {
            _locationRepository = Injector.CreateInstance<ILocationRepository>();
        }

        public IEnumerable<Location> GetLocations()
        {
            return _locationRepository.GetAll();
        }

        public IEnumerable<Location> GetAll()
        {
            return _locationRepository.GetAll();
        }

        public Location GetByCountryAndCity(string country, string city)
        {
            return _locationRepository.GetAll().Find(l => l.Country == country && l.City == city);
        }

        public IEnumerable<string> GetAllCountries()
        {
            return _locationRepository.GetAll().Select(l => l.Country).ToHashSet();
        }

        public Location GetLocationById(int id)
        {
            return _locationRepository.GetById(id);
        }
    }
}
