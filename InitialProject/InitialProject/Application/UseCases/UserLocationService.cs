using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class UserLocationService
    {
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly LocationService _locationService;
        private readonly AccommodationService _accommodationService;

        public UserLocationService()
        {
            _accommodationReservationService = new AccommodationReservationService();
            _locationService = new LocationService();
            _accommodationService = new AccommodationService();
        }

        public bool WasUserOnThisLocation(int guestId, int locationId, int ownerId)
        {
            if (guestId == ownerId)
            {
                return true;
            }

            IEnumerable<AccommodationReservation> guestsReservations = _accommodationReservationService.GetGuestsReservations(guestId);

            guestsReservations = _accommodationReservationService.LoadAccommodations(guestsReservations);

            return guestsReservations.FirstOrDefault(gr => gr.Accommodation.LocationId == locationId) != null;
        }

        public bool IsThisOwnersLocation(int ownerId, int locationId)
        {
            IEnumerable<Accommodation> ownersAccommodations = _accommodationService.GetByOwnerId(ownerId);

            foreach (var accommodation in ownersAccommodations)
            {
                if (accommodation.LocationId == locationId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
