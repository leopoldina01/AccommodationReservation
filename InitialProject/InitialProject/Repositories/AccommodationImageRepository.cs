using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class AccommodationImageRepository : IAccommodationImageRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationImages.csv";

        private readonly Serializer<AccommodationImage> _serializer;

        private List<AccommodationImage> _images;

        public AccommodationImageRepository()
        {
            _serializer = new Serializer<AccommodationImage>();
            _images = _serializer.FromCSV(FilePath);
        }

        public List<AccommodationImage> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AccommodationImage Save(string url, int accommodationId)
        {
            int id = NextId();

            AccommodationImage image = new AccommodationImage(id, url, accommodationId);
            _images.Add(image);
            SaveAllImages();
            return image;
        }

        public List<AccommodationImage> LoadAllImages()
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

        public void AddAccommodationId(int accommodationId)
        {
            _images = LoadAllImages();

            foreach (var image in _images)
            {
                if (image.AccommodationId == -1)
                {
                    image.AccommodationId = accommodationId;
                }
            }

            SaveAllImages();
        }

        public void RemovePicturesForCanceledAccommodation()
        {
            _images = LoadAllImages();

            _images.RemoveAll(image => image.AccommodationId == -1);

            SaveAllImages();
        }
    }
}
