using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain.Models;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class SuperGuestService
    {
        private readonly ISuperGuestRepository _superGuestRepository;
        private readonly AccommodationReservationService _reservationService;

        public SuperGuestService()
        {
            _superGuestRepository = Injector.CreateInstance<ISuperGuestRepository>();

            _reservationService = new AccommodationReservationService();
        }

        public void ManageGuestRole(User user)
        {
            var lastYearReservations = _reservationService.GetLastYearReservations(user);
            if (lastYearReservations.Count() > 9 && !_superGuestRepository.GetAll().Exists(x => x.GuestId == user.Id))
            {
                _superGuestRepository.Save(user.Id, 5, DateTime.Now.AddDays(365));
            }
            else if (_superGuestRepository.GetAll().Exists(x => x.GuestId == user.Id && DateTime.Compare(DateTime.Now, x.ExpirationDate) > 0))
            {
                if (lastYearReservations.Count() > 9)
                    _superGuestRepository.Update(user.Id, 5, DateTime.Now.AddDays(365));
                else
                    _superGuestRepository.Remove(user.Id);
            }
        }

        public void RemoveBonusPoint(int guestId)
        {
            var superGuest = GetGuestByUserId(guestId);
            if (superGuest == null)
                return;
            if (superGuest.BonusPoints > 0)
                _superGuestRepository.Update(superGuest.GuestId, superGuest.BonusPoints - 1, superGuest.ExpirationDate);
        }

        public SuperGuest GetGuestByUserId(int userId)
        {
            var superGuests = _superGuestRepository.GetAll();
            return superGuests.Find(x => x.GuestId == userId);
        }

    }
}
