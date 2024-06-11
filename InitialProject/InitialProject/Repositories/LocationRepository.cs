using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private const string FilePath = "../../../Resources/Data/locations.csv";

        private readonly Serializer<Location> _serializer;

        private List<Location> _locations;

        public LocationRepository()
        {
            _serializer = new Serializer<Location>();
            _locations = _serializer.FromCSV(FilePath);
        }

        public List<Location> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<string> GetAllCountries()
        {
            List<string> countries = new List<string>();

            foreach (var location in _locations)
            {
                countries.Add(location.Country);
            }

            countries = countries.Distinct().ToList();

            return countries;
        }

        public Location Save(string city, string country)
        {
            int id = NextId();

            Location location = new Location(id, city, country);
            _locations.Add(location);
            _serializer.ToCSV(FilePath, _locations);
            return location;
        }

        public int NextId()
        {
            _locations = _serializer.FromCSV(FilePath);
            if (_locations.Count < 1)
            {
                return 1;
            }

            return _locations.Max(c => c.Id) + 1;
        }

        public Location GetById(int id)
        {
            _locations = _serializer.FromCSV(FilePath);
            foreach (Location location in _locations)
            {
                if (location.Id == id)
                {
                    return location;
                }
            }
            return null;
        }

        public Location GetByCountryAndCity(string country, string city)
        {
            _locations = _serializer.FromCSV(FilePath);
            foreach (var location in _locations)
            {
                if (location.City.Equals(city) && location.Country.Equals(country)) return location;
            }
            return null;
        }

        public List<string> GetCitiesByCountry(string country)
        {
            _locations = _serializer.FromCSV(FilePath);
            List<string> cities = new List<string>();

            foreach (var location in _locations)
            {
                if (location.Country == country)
                {
                    cities.Add(location.City);
                }
            }

            return cities;
        }
    }
}
