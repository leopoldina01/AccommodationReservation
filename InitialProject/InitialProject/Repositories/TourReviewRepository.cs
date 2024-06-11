using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace InitialProject.Repositories
{
    public class TourReviewRepository : ITourReviewRepository
    {
        private const string FilePath = "../../../Resources/Data/tourReviews.csv";
        private readonly Serializer<TourReview> _serializer;
        private readonly ITourReviewImageRepository _tourRatingImageRepository;
        private List<TourReview> _reviews;

        public TourReviewRepository()
        {
            _serializer = new Serializer<TourReview>();
            _reviews = _serializer.FromCSV(FilePath);
            //_tourRatingImageRepository = Injector.CreateInstance<ITourRatingImageRepository>();
            _tourRatingImageRepository = new TourReviewImageRepository();
        }

        public void Delete(TourReview review)
        {
            _reviews = _serializer.FromCSV(FilePath);
            TourReview found = _reviews.Find(t => t.Id == review.Id);
            _reviews.Remove(found);
            _serializer.ToCSV(FilePath, _reviews);
        }

        public List<TourReview> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _reviews = _serializer.FromCSV(FilePath);
            if (_reviews.Count < 1)
            {
                return 1;
            }
            return _reviews.Max(t => t.Id) + 1;
        }

        public TourReview Save(TourReview review, List<BitmapImage> images)
        {
            int id = NextId();
            review.Id = id;
            foreach(var image in  images)
            {
                _tourRatingImageRepository.Save(image.ToString(), id);
            }
            _reviews = _serializer.FromCSV(FilePath);
            _reviews.Add(review);
            _serializer.ToCSV(FilePath, _reviews);
            return review;
        }

        public TourReview Update(TourReview review)
        {
            _reviews = _serializer.FromCSV(FilePath);
            TourReview current = _reviews.Find(t => t.Id == review.Id);
            int index = _reviews.IndexOf(current);
            _reviews.Remove(current);
            _reviews.Insert(index, review);
            _serializer.ToCSV(FilePath, _reviews);
            return review;
        }
    }
}
