using InitialProject.Domain.DTOs;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Security.Policy;

namespace InitialProject.Application.UseCases
{
    public class TourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ICheckpointRepository _checkpointRepository;
        private readonly ITourReservationRepository _tourReservationRepository;
        private readonly LocationService _locationService;

        public TourService()
        {
            _tourRepository = Injector.CreateInstance<ITourRepository>();
            _locationRepository = Injector.CreateInstance<ILocationRepository>();
            _checkpointRepository = Injector.CreateInstance<ICheckpointRepository>();
            _tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            _locationService = new LocationService();
        }

        public IEnumerable<Tour> GetPastTours()
        {
            var pastTours = _tourRepository.GetAll().Where(t => t.StartTime.Subtract(DateTime.Now) < TimeSpan.Zero);
            LoadLocations(pastTours);
            return pastTours;
        }

        public IEnumerable<Tour> GetFutureToursByGuide(User guide)
        {
            var futureTours = _tourRepository.GetAll().Where(t => t.StartTime.Subtract(DateTime.Now) > TimeSpan.Zero && t.GuideId == guide.Id);
            LoadLocations(futureTours);
            return futureTours;
        }

        public IEnumerable<Tour> GetPastToursByGuide(User guide)
        {
            var pastTours = _tourRepository.GetAll().Where(t => t.StartTime.Subtract(DateTime.Now) < TimeSpan.Zero && t.GuideId == guide.Id);
            LoadLocations(pastTours);
            return pastTours;
        }

        public void CancelTour(Tour tour)
        {
            tour.Status = TourStatus.CANCELED;
            _tourRepository.Update(tour);
        }

        public void UncancelTour(Tour tour)
        {
            tour.Status = TourStatus.NOT_STARTED;
            _tourRepository.Update(tour);
        }

        public IEnumerable<TourCheckpoint> GetReservedTours(User user)
        {
            var reservedTours = new List<TourCheckpoint>();
            var checkpoints = _checkpointRepository.GetAll();
            var usersReservedTours = _tourReservationRepository.GetAll().Where(t => t.UserId == user.Id);

            foreach (var reservation in usersReservedTours)
            {
                foreach(var checkpoint in checkpoints)
                {
                    if(checkpoint.TourId == reservation.TourId && checkpoint.Active == true && reservation.UserId == user.Id)
                    {
                        Tour tour = _tourRepository.GetById(reservation.TourId); //Podrazumeva se da ce se prikazati samo aktivne i finished jer su samo u tim slucajevima checkpointi active!
                        var ReservedTour = new TourCheckpoint(tour.Id, tour.Name, tour.Status, checkpoint.Id, checkpoint.Name, user.Id);
                        reservedTours.Add(ReservedTour);
                    }
                }
            }
            return reservedTours;
        }

        private void LoadLocations(IEnumerable<Tour> tours)
        {
            foreach (var tour in tours)
            {
                tour.Location = _locationRepository.GetById(tour.LocationId);
            }
        }

        public int GetGuideId(int tourId)
        {
            var tours = _tourRepository.GetAll();
            foreach (var tour in tours)
            {
                if(tour.Id == tourId)
                {
                    return tour.GuideId;
                }
            }
            return -1;
        }

        public Tour GetById(int id)
        {
            var tour = _tourRepository.GetAll().FirstOrDefault(t => t.Id == id);
            tour.Location = _locationRepository.GetById(tour.LocationId);
            return tour;
        }

        public IEnumerable<DateTime> GetAllStartTimesForTour(Tour tour)
        {
            return _tourRepository.GetAll().Where(t => t.Name == tour.Name).Select(t => t.StartTime);
        }

        public IEnumerable<Tour> GetTodaysToursByGuideId(int guideId)
        {
            var tours = _tourRepository.GetAll().Where(t => t.GuideId == guideId && t.StartTime.Date == DateTime.Now.Date);
            LoadLocations(tours);
            return tours;
        }

        public bool IsTourActive(Tour tour)
        {
            return tour.Status == TourStatus.ACTIVE;
        }

        public void ActivateTour(Tour tour)
        {
            tour.Status = TourStatus.ACTIVE;
            _tourRepository.Update(tour);
        }

        public void FinishTour(Tour tour)
        {
            tour.Status = TourStatus.FINISHED;
            _tourRepository.Update(tour);
        }

        public Tour Create(string name, string country, string city, string description, string language, int maxGuests, DateTime startTime, int duration, string coverImageUrl, int guideId)
        {
            Location location = _locationService.GetByCountryAndCity(country, city);
            return _tourRepository.Create(name, location, location.Id, description, language, maxGuests, startTime, duration, coverImageUrl, guideId);
        }

        public IEnumerable<string> GetCountries()
        {
            List<string> countries = new List<string>();
            foreach (var location in _locationRepository.GetAll())
            {
                if (!countries.Contains(location.Country))
                {
                    countries.Add(location.Country);
                }
            }
            return countries;
        }

        public IEnumerable<string> GetCities(string selectedCountry)
        {
            List<string> cities = new List<string>();
            foreach (var location in _locationRepository.GetAll())
            {
                if (selectedCountry == location.Country)
                {
                    cities.Add(location.City);
                }
            }
            return cities;
        }

        public IEnumerable<Tour> GetTours()
        {
            return _tourRepository.GetAll();
        }

        public void UpdateTour(Tour tour)
        {
            _tourRepository.Update(tour);
        }

        public bool IsGuideFree(User guide, DateTime date)
        {
            foreach (var tour in GetAllByGuide(guide))
            {
                if (tour.StartTime.Date == date.Date) return false;
            }
            return true;
        }

        public IEnumerable<Tour> GetAllByGuide(User guide)
        {
            var tours = _tourRepository.GetAll().Where(t => t.GuideId == guide.Id);
            LoadLocations(tours);
            return tours;
        }

        public IEnumerable<Tour> GetAllForGuideAndLanguageInLastYear(User guide, string language)
        {
            var tours =  this.GetPastToursByGuide(guide).Where(t => t.Language.Equals(language) && DateTime.Compare(t.StartTime, DateTime.Now.Subtract(TimeSpan.FromDays(365))) > 0);
            LoadLocations(tours);
            return tours;
        }
    }
}
