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
    public class AccommodationRatingImageRepository : IAccommodationRatingImageRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationRatingImages.csv";

        private readonly Serializer<AccommodationRatingImage> _serializer;

        private List<AccommodationRatingImage> _images;

        public AccommodationRatingImageRepository()
        {
            _serializer = new Serializer<AccommodationRatingImage>();
            _images = _serializer.FromCSV(FilePath);
        }

        public List<AccommodationRatingImage> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AccommodationRatingImage Save(string url, int accommodationRatingId)
        {
            int id = NextId();

            AccommodationRatingImage image = new AccommodationRatingImage(id, url, accommodationRatingId);
            _images.Add(image);
            SaveAllImages();
            return image;
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

        public IEnumerable<AccommodationRatingImage> GetAllByRatingId(int ratingId)
        {
            _images = _serializer.FromCSV(FilePath);

            return _images.FindAll(i => i.AccommodationRatingId == ratingId);
        }
    }
}
