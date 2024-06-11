using InitialProject.Domain.Models;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourReviewRepository
    {
        public List<TourReview> GetAll();
        public TourReview Save(TourReview review, List<BitmapImage> images);
        public int NextId();
        public void Delete(TourReview review);
        public TourReview Update(TourReview review);
    }
}
