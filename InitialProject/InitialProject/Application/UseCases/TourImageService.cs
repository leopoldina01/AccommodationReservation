using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class TourImageService
    {
        private readonly ITourImageRepository _tourImageRepository;

        public TourImageService()
        {
            _tourImageRepository = Injector.CreateInstance<ITourImageRepository>();
        }

        public IEnumerable<TourImage> GetAllByTour(Tour tour)
        {
            return _tourImageRepository.GetAll().Where(i => i.TourId == tour.Id);
        }

        public TourImage Create(string url,Tour tour)
        {
            return _tourImageRepository.Save(url, tour.Id);
        }
    }
}
