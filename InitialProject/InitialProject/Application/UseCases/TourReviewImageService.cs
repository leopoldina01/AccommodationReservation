using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace InitialProject.Application.UseCases
{
    public class TourReviewImageService
    {
        private readonly ITourReviewImageRepository _tourReviewImageRepository;

        public TourReviewImageService()
        {
            _tourReviewImageRepository = Injector.CreateInstance<ITourReviewImageRepository>();
        }

        public void SaveImage(string url, int reviewId)
        {
            _tourReviewImageRepository.Save(url, reviewId);
        }

        public IEnumerable<TourReviewImage> GetAllByReview(TourReview review)
        {
            return _tourReviewImageRepository.GetAll().Where(i => i.ReviewId == review.Id);
        }
    }
}
