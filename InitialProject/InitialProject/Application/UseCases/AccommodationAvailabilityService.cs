using InitialProject.Domain.DTOs;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using InitialProject.WPF.Views.Guest1Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class AccommodationAvailabilityService
    {
        private readonly IAccommodationReservationRepository _accommodationReservationRepository;
        private readonly AccommodationService _accommodationService;

        public AccommodationAvailabilityService()
        {
            _accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            _accommodationService = new AccommodationService();
        }

        public string IsAvailable(DateTime newStartDate, DateTime newEndDate, int reservationId, int accommodationId)
        {
            List<DateTime> allSingleDates = FindDatesBetween(newStartDate, newEndDate);

            foreach (var date in allSingleDates)
            {
                if (!IsSingleDateAvailable(date, reservationId, accommodationId))
                {
                    return "no";
                }
            }

            return "yes";
        }

        private bool IsSingleDateAvailable(DateTime date, int reservationId, int accommodationId)
        {
            foreach (var accommodationReservation in _accommodationReservationRepository.GetAll())
            {
                if (accommodationReservation.Id != reservationId && accommodationReservation.AccommodationId == accommodationId)
                {
                    if (FindDatesBetween(accommodationReservation.StartDate, accommodationReservation.EndDate).Contains(date))
                    {
                        return false;
                    }
                }
            }

            return true;
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

        public List<AccommodationInfo> GetAllAvailableAccommodations(DateTime startDate, DateTime endDate, int durationOfStay)
        {
            List<DateTime> allSingleDates = FindDatesBetween(startDate, endDate);
            List<AccommodationInfo> availableAccommodations = GetAllComplexAccommodations(startDate, endDate);

            foreach (var date in allSingleDates)
            {
                foreach (var accommodation in availableAccommodations)
                {
                    RemoveIfUnavailable(date, accommodation.Dates, accommodation.Id);
                    accommodation.AvailableDates = FindConnectedDates(accommodation.Dates, endDate, durationOfStay);
                }
            }

            return availableAccommodations;
        }

        private void RemoveIfUnavailable(DateTime date, List<DateTime> dates, int accommodationId)
        {
            foreach (var accommodationReservation in _accommodationReservationRepository.GetAll())
            {
                if (FindDatesBetween(accommodationReservation.StartDate, accommodationReservation.EndDate).Contains(date) && accommodationReservation.AccommodationId == accommodationId)
                {
                    dates.Remove(date);
                }
            }
        }

        private List<AccommodationInfo> GetAllComplexAccommodations(DateTime startDate, DateTime endDate)
        {
            List<AccommodationInfo> complexAccommodations = new List<AccommodationInfo>(); 
            foreach (var accommodation in _accommodationService.GetAll())
            {
                complexAccommodations.Add(new AccommodationInfo(accommodation.Id, accommodation.Name, accommodation.Location.Country, accommodation.Location.City, accommodation.Type.ToString(), accommodation.Capacity, accommodation.MinDaysForStay, startDate, endDate, FindDatesBetween(startDate, endDate)));
            }

            return complexAccommodations;
        }

        private List<AvailableDate> FindConnectedDates(List<DateTime> singleDates, DateTime finishEndDate, int durationOfStay)
        {
            List<AvailableDate> connectedDates = new List<AvailableDate>();
            foreach (var singleDate in singleDates)
            {
                AvailableDate newDate = new AvailableDate(singleDate, singleDate.AddDays(Convert.ToInt32(durationOfStay) - 1));
                foreach (var date in FindDatesBetween(newDate.StartDate, newDate.EndDate))
                {
                    if (!singleDates.Contains(date))
                    {
                        break;
                    }
                    else if (date == newDate.EndDate)
                    {
                        if (DateTime.Compare(newDate.EndDate, finishEndDate) <= 0)
                        {
                            connectedDates.Add(newDate);
                        }
                    }
                }
            }

            return connectedDates;
        }
    }
}
