using InitialProject.Domain.Models;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ILocationRepository
    {
        public List<Location> GetAll();
        public List<string> GetAllCountries();
        public Location Save(string city, string country);
        public int NextId();
        public Location GetById(int id);
        public Location GetByCountryAndCity(string country, string city);
        public List<string> GetCitiesByCountry(string country);
    }
}
