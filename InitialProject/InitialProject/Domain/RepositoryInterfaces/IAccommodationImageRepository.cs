using InitialProject.Domain.Models;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationImageRepository
    {
        public List<AccommodationImage> GetAll();
        public AccommodationImage Save(string url, int accommodationId);
        public List<AccommodationImage> LoadAllImages();
        public void SaveAllImages();
        public int NextId();
        public void AddAccommodationId(int accommodationId);
        public void RemovePicturesForCanceledAccommodation();
    }
}
