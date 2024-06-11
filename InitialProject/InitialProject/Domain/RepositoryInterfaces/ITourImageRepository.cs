using InitialProject.Domain.Models;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourImageRepository
    {
        public List<TourImage> GetAll();
        public TourImage Save(string url, int tourId);
        public List<TourImage> LoadAllImages();
        public void SaveAllImages();
        public int NextId();
        public void AddTourId(int tourId);
    }
}
