using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class AccommodationRatingImageService
    {
        private readonly IAccommodationRatingImageRepository _accommodationRatingImageRepository;

        public AccommodationRatingImageService() 
        {
            _accommodationRatingImageRepository = Injector.CreateInstance<IAccommodationRatingImageRepository>();
        }

        public void SaveImage(string url, int accommodationRatingId)
        {
            _accommodationRatingImageRepository.Save(url, accommodationRatingId);
        }

        public IEnumerable<AccommodationRatingImage> GetAllByRatingId(int ratingId)
        {
            return _accommodationRatingImageRepository.GetAllByRatingId(ratingId);
        }
    }
}
