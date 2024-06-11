using InitialProject.Domain.Models;
using InitialProject.Repositories;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationReservationRepository
    {
        public List<AccommodationReservation> GetAll();
        public AccommodationReservation Save(DateTime startDate, DateTime endDate, int lenghtOfStay, Accommodation accommodation, int accommodationId, User guest, int guestId, string status);
        public int NextId();
        public List<AccommodationReservation> GetAllByOwnerId(int ownerId, AccommodationRepository accommodationRepository, UserRepository userRepository);
        public List<AccommodationReservation> GetAllByGuestId(int guestId);
        public void Remove(AccommodationReservation reservation);
        public AccommodationReservation GetById(int reservationId);
        public AccommodationReservation Update(AccommodationReservation accommodationReservation);
        public List<AccommodationReservation> GetCancelledByGuestId(int guestId);
    }
}
