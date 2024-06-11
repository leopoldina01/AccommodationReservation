using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationRatingImageRepository
    {
        public List<AccommodationRatingImage> GetAll();
        public AccommodationRatingImage Save(string url, int accommodationRatingId);
        public void SaveAllImages();
        public int NextId();
        public IEnumerable<AccommodationRatingImage> GetAllByRatingId(int ratingId);
    }
}
