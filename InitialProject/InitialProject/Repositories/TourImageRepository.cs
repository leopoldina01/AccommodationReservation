using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class TourImageRepository : ITourImageRepository
    {
        private const string FilePath = "../../../Resources/Data/tourImages.csv";

        private readonly Serializer<TourImage> _serializer;

        private List<TourImage> _images;

        public TourImageRepository()
        {
            _serializer = new Serializer<TourImage>();
            _images = _serializer.FromCSV(FilePath);
        }

        public List<TourImage> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public TourImage Save(string url, int tourId)
        {
            int id = NextId();

            TourImage image = new TourImage(id, url, tourId);
            _images.Add(image);
            SaveAllImages();
            return image;
        }

        public List<TourImage> LoadAllImages()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void SaveAllImages()
        {
            _serializer.ToCSV(FilePath, _images);
        }

        public int NextId()
        {
            _images = _serializer.FromCSV(FilePath);

            if (_images.Count < 1)
            {
                return 1;
            }

            return _images.Max(c => c.Id) + 1;
        }

        public void AddTourId(int tourId)
        {
            _images = LoadAllImages();

            foreach (var image in _images)
            {
                if (image.TourId == -1)
                {
                    image.TourId = tourId;
                }
            }

            SaveAllImages();
        }

        /*public void RemovePicturesForCanceledAccommodation()
        {
            _images = LoadAllImages();

            _images.RemoveAll(image => image.AccommodationId == -1);

            SaveAllImages();
        }*/
    }
}
