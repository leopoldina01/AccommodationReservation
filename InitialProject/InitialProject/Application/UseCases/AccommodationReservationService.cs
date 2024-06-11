using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InitialProject.Application.UseCases
{

    public class AccommodationReservationService
    {
        private readonly IAccommodationReservationRepository _accommodationReservationRepository;
        private readonly IGuestRatingRepository _ratingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly ILocationRepository _locationRepository;

        private readonly AccommodationAvailabilityService _accommodationAvailabilityService;

        public AccommodationReservationService()
        {
            _accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            _ratingRepository = Injector.CreateInstance<IGuestRatingRepository>();
            _userRepository = Injector.CreateInstance<IUserRepository>();
            _accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            _locationRepository = Injector.CreateInstance<ILocationRepository>();

            _accommodationAvailabilityService = new AccommodationAvailabilityService();
        }

        public IEnumerable<AccommodationReservation> GetRatedReservations(int ownerId)
        {
            List<AccommodationReservation> ownerReservations = new List<AccommodationReservation>();
            List<int> accommodationIdsForOwner = _accommodationRepository.AccommodationIdsByOwnerId(ownerId);

            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                if (accommodationIdsForOwner.Find(a => a == reservation.AccommodationId) != 0)
                {
                    reservation.Guest = _userRepository.GetAll().Find(g => g.Id == reservation.GuestId);
                    ownerReservations.Add(reservation);
                }
            }

            var ratedReservations = RemoveUnratedReservations(ownerReservations);

            ratedReservations = LoadAccommodations(ratedReservations);

            return ratedReservations;
        }

        public IEnumerable<AccommodationReservation> RemoveUnratedReservations(List<AccommodationReservation> ownerReservations)
        {
            var filteredOwnerReservations = new List<AccommodationReservation>();

            foreach (var reservation in ownerReservations)
            {
                if (_ratingRepository.GetByReservationId(reservation.Id) != null)
                {
                    filteredOwnerReservations.Add(reservation);
                }
            }

            return filteredOwnerReservations;
        }

        public List<AccommodationReservation> LoadAccommodations(IEnumerable<AccommodationReservation> ratedReservations)
        {
            var updatedRatedReservations = new List<AccommodationReservation>();

            foreach (var reservation in ratedReservations)
            {
                reservation.Accommodation = _accommodationRepository.GetById(reservation.AccommodationId);
                reservation.Accommodation.Location = _locationRepository.GetById(reservation.Accommodation.LocationId);
                updatedRatedReservations.Add(reservation);
            }

            return updatedRatedReservations;
        }

        public void LoadAccommodation(AccommodationReservation reservation)
        {
            reservation.Accommodation = _accommodationRepository.GetById(reservation.AccommodationId);
            reservation.Accommodation.Location = _locationRepository.GetById(reservation.Accommodation.LocationId);
        }

        private List<AccommodationReservation> LoadGuests(IEnumerable<AccommodationReservation> oldReservations)
        {
            var updatedReservations = new List<AccommodationReservation>();

            foreach (var reservation in oldReservations)
            {
                reservation.Guest = _userRepository.GetById(reservation.GuestId);
                updatedReservations.Add(reservation);
            }

            return updatedReservations;
        }

        public IEnumerable<AccommodationReservation> GetGuestsReservations(int guestId)
        {
            var guestReservations = _accommodationReservationRepository.GetAllByGuestId(guestId);
            guestReservations = LoadAccommodations(guestReservations);
            guestReservations = LoadGuests(guestReservations);

            return guestReservations;
        }

        public IEnumerable<AccommodationReservation> GetGuestsCancelledReservations(int guestId)
        {
            var guestReservations = _accommodationReservationRepository.GetCancelledByGuestId(guestId);
            guestReservations = LoadAccommodations(guestReservations);
            guestReservations = LoadGuests(guestReservations);

            return guestReservations;
        }

        public void CancelReservation(AccommodationReservation reservation)
        {
            reservation.Status = "Cancelled";
            _accommodationReservationRepository.Update(reservation);
        }

        public bool IsAccommodationAvailable(DateTime startDate, DateTime endDate, int reservationId, int accommodationId)
        {
            string yesNoanswer = _accommodationAvailabilityService.IsAvailable(startDate, endDate, reservationId, accommodationId);
            if (yesNoanswer.Equals("yes")) return true; else return false;
        }

        public IEnumerable<AccommodationReservation> GetAllByOwnerId(int ownerId)
        {
            List<AccommodationReservation> ownerReservations = new List<AccommodationReservation>();
            List<int> accommodationIdsForOwner = _accommodationRepository.AccommodationIdsByOwnerId(ownerId);

            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                if (accommodationIdsForOwner.Find(a => a == reservation.AccommodationId) != 0)
                {
                    reservation.Guest = _userRepository.GetAll().Find(g => g.Id == reservation.GuestId);
                    ownerReservations.Add(reservation);
                }
            }

            ownerReservations = LoadAccommodations(ownerReservations);
            return ownerReservations;
        }

        public IEnumerable<AccommodationReservation> GetByAccommodationId(int accommodationId)
        {
            return _accommodationReservationRepository.GetAll().FindAll(ar => ar.AccommodationId == accommodationId);
        }

        public int GetNumberOfTakenDaysInYearByAccommodationId(int year, int accommodationId)
        {
            List<AccommodationReservation> accommodationReservations = _accommodationReservationRepository.GetAll().FindAll(ar => ar.AccommodationId == accommodationId && (ar.StartDate.Year == year || ar.EndDate.Year == year));

            int numberOfTakenDays = 0;

            foreach(var reservation in accommodationReservations)
            {
                if (reservation.StartDate.Year == year && reservation.EndDate.Year == year)
                {
                    numberOfTakenDays += reservation.LenghtOfStay;
                }
                else if (reservation.StartDate.Year != year && reservation.EndDate.Year == year)
                {
                    numberOfTakenDays += SumDates(reservation, year);
                }
                else if (reservation.StartDate.Year == year && reservation.EndDate.Year != year)
                {
                    numberOfTakenDays += SumDates(reservation, year);
                }
            }

            return numberOfTakenDays;
        }

        private int SumDates(AccommodationReservation reservation, int year)
        {
            int numberOfDaysAfter = 0;

            List<DateTime> allSingleDates = FindDatesBetween(reservation.StartDate, reservation.EndDate);

            foreach (var date in allSingleDates)
            {
                if (date.Year == year)
                {
                    numberOfDaysAfter++;
                }
            }

            return numberOfDaysAfter;
        }

        private List<DateTime> FindDatesBetween(DateTime startDate, DateTime endDate)
        {
            List<DateTime> resultingDates = new List<DateTime>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                resultingDates.Add(date);
            }

            return resultingDates;
        }

        public int GetNumberOfTakenDaysInMonthByAccommodationId(int month, AccommodationYearStatistic yearStatistics)
        {
            List<AccommodationReservation> accommodationReservations = _accommodationReservationRepository.GetAll().FindAll(ar => ar.AccommodationId == yearStatistics.AccommodationId && (ar.StartDate.Month == month || ar.EndDate.Month == month));
            
            int numberOfTakenDays = 0;

            foreach (var reservation in accommodationReservations)
            {
                if (reservation.StartDate.Month == month && reservation.EndDate.Month == month)
                {
                    numberOfTakenDays += reservation.LenghtOfStay;
                }
                else if (reservation.StartDate.Month != month && reservation.EndDate.Month == month)
                {
                    numberOfTakenDays += SumDatesInMonth(reservation, month);
                }
                else if (reservation.StartDate.Month == month && reservation.EndDate.Month != month)
                {
                    numberOfTakenDays += SumDatesInMonth(reservation, month);
                }
            }

            return numberOfTakenDays;
        }

        private int SumDatesInMonth(AccommodationReservation reservation, int month)
        {
            int numberOfDaysAfter = 0;

            List<DateTime> allSingleDates = FindDatesBetween(reservation.StartDate, reservation.EndDate);

            foreach (var date in allSingleDates)
            {
                if (date.Month == month)
                {
                    numberOfDaysAfter++;
                }
            }

            return numberOfDaysAfter;
        }

        public AccommodationReservation GetById(int reservationId)
        {
            return _accommodationReservationRepository.GetById(reservationId);
        }

        public IEnumerable<AccommodationReservation> GetLastYearReservations(User user)
        {
            var guestsReservations = GetGuestsReservations(user.Id);
            var lastYearReservations = new List<AccommodationReservation>();
            foreach (var reservation in guestsReservations)
            {
                if ((DateTime.Now.Date - reservation.EndDate.Date).Days < 365)
                    lastYearReservations.Add(reservation);
            }
            return lastYearReservations;
        }

        public AccommodationReservation Save(DateTime startDate, DateTime endDate, int lenghtOfStay, Accommodation accommodation, int accommodationId, User guest, int guestId, string status)
        {
            return _accommodationReservationRepository.Save(startDate, endDate, lenghtOfStay, accommodation, accommodationId, guest, guestId, status);
        }
    }
}
