using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourReviewImageRepository
    {
        public List<TourReviewImage> GetAll();
        public TourReviewImage Save(string url, int reviewId);
        public List<TourReviewImage> LoadAllImages();
        public void SaveAllImages();
        public int NextId();
    }
}
