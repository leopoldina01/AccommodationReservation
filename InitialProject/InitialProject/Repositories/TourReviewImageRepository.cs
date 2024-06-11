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
    public class TourReviewImageRepository : ITourReviewImageRepository
    {
        private const string FilePath = "../../../Resources/Data/tourReviewImages.csv";
        private readonly Serializer<TourReviewImage> _serializer;

        private List<TourReviewImage> _images;

        public TourReviewImageRepository()
        {
            _serializer = new Serializer<TourReviewImage>();
            _images = _serializer.FromCSV(FilePath);
        }

        public List<TourReviewImage> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<TourReviewImage> LoadAllImages()
        {
            return _serializer.FromCSV(FilePath);
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

        public TourReviewImage Save(string url, int reviewId)
        {
            int id = NextId();
            
            TourReviewImage image = new TourReviewImage(url, reviewId);
            image.Id = id;
            _images.Add(image);
            SaveAllImages();
            return image;
        }

        public void SaveAllImages()
        {
            _serializer.ToCSV(FilePath, _images);
        }
    }
}
