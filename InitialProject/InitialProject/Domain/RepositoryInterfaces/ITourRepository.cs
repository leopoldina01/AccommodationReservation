using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourRepository
    {
        public List<Tour> GetAll();
        public Tour Save(Tour tour);
        public int NextId();
        public void Delete(Tour tour);
        public Tour Update(Tour tour);
        public Tour Create(string name, Location location, int locationId, string description, string language, int maxGuests, DateTime startTime, double duration, string coverImageUrl, int guideId);
        public Tour GetById(int id);
    }
}
