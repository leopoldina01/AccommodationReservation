using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class AccommodationImageService
    {
        private readonly IAccommodationImageRepository _accommodationImageRepository;

        public AccommodationImageService()
        {
            _accommodationImageRepository = Injector.CreateInstance<IAccommodationImageRepository>();
        }

        public IEnumerable<AccommodationImage> GetAllByAccommodationId(int accommodationId)
        {
            return _accommodationImageRepository.GetAll().FindAll(ai => ai.AccommodationId == accommodationId);
        }
    }
}
