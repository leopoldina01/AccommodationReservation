using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class AccommodationRenovationService
    {
        private readonly IAccommodationRenovationRepository _accommodationRenovationRepository;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly LocationService _locationService;

        public AccommodationRenovationService()
        {
            _accommodationRenovationRepository = Injector.CreateInstance<IAccommodationRenovationRepository>();
            _accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            _locationService = new LocationService();
        }

        public AccommodationRenovation Save(Accommodation accommodation, DateTime startDate, DateTime endDate, int renovationLenght, string comment)
        {
            return _accommodationRenovationRepository.Save(accommodation, startDate, endDate, renovationLenght, comment);
        }

        public List<AccommodationRenovation> GetAll()
        {
            return _accommodationRenovationRepository.GetAll();
        }

        internal IEnumerable<AccommodationRenovation> GetByAccommodationId(int accommodationId)
        {
            return _accommodationRenovationRepository.GetAll().FindAll(ar => ar.AccommodationId == accommodationId);
        }

        internal IEnumerable<AccommodationRenovation> GetByOwnerId(int ownerId)
        {
            var ownersRenovations = _accommodationRenovationRepository.GetAll();

            ownersRenovations = LoadAccommodations(ownersRenovations);

            return ownersRenovations.FindAll(or => or.Accommodation.OwnerId == ownerId);
        }

        private List<AccommodationRenovation> LoadAccommodations(IEnumerable<AccommodationRenovation> ownersRenovations)
        {
            var updatedOwnerRenovations = new List<AccommodationRenovation>();

            foreach (var ownerRenovation in ownersRenovations)
            {
                ownerRenovation.Accommodation = _accommodationRepository.GetById(ownerRenovation.AccommodationId);
                ownerRenovation.Accommodation.Location = _locationService.GetLocationById(ownerRenovation.Accommodation.LocationId);
                updatedOwnerRenovations.Add(ownerRenovation);
            }

            return updatedOwnerRenovations;
        }

        public void Update(AccommodationRenovation updatedRenovation)
        {
            _accommodationRenovationRepository.Update(updatedRenovation);
        }

        public List<AccommodationRenovation> GetAllFinishedToday()
        {
            return _accommodationRenovationRepository.GetAll().FindAll(ar => ar.EndDate.Date == DateTime.Now.Date);
        }

        internal List<AccommodationRenovation> GetAllFinishedTodayAndNotMarked()
        {
            List<AccommodationRenovation> finishedRenovationsToday = GetAllFinishedToday();

            finishedRenovationsToday = LoadAccommodations(finishedRenovationsToday);

            return finishedRenovationsToday.FindAll(ft => ft.Accommodation.RenovationStatus == "");
        }

        public List<AccommodationRenovation> GetAllFinishedInLastYear()
        {
            return _accommodationRenovationRepository.GetAll().FindAll(ar => (DateTime.Now.Date - ar.EndDate.Date).Days < 365 && (DateTime.Now.Date - ar.EndDate.Date).Days > 0);
        }

        public List<AccommodationRenovation> GetAllFinishedInLastYearAndNotMarked()
        {
            List<AccommodationRenovation> finishedInLastYear = GetAllFinishedInLastYear();

            finishedInLastYear = LoadAccommodations(finishedInLastYear);

            return finishedInLastYear.FindAll(fl => fl.Accommodation.RenovationStatus == "");
        }

        internal List<AccommodationRenovation> GetAllRenovatedBeforeMoreThanAYear()
        {
            List<AccommodationRenovation> finishedBeforeMoreThanAYear = _accommodationRenovationRepository.GetAll().FindAll(fb => (DateTime.Now.Date - fb.EndDate.Date).Days >= 365);

            finishedBeforeMoreThanAYear = LoadAccommodations(finishedBeforeMoreThanAYear);

            return finishedBeforeMoreThanAYear;
        }

        public void Delete(AccommodationRenovation accommodationRenovation)
        {
            _accommodationRenovationRepository.Delete(accommodationRenovation);
        }
    }
}
