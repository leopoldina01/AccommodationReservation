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
    public class ManageRequestService
    {
        private readonly IReservationRequestRepository _requestRepository;
        private readonly IAccommodationReservationRepository _accommodationReservationRepository;

        public ManageRequestService() 
        {
            _requestRepository = Injector.CreateInstance<IReservationRequestRepository>();
            _accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
        }

        public void DeclineRequest(ReservationRequest selectedRequest)
        {
            ReservationRequest request = selectedRequest;

            request.Status = RequestStatus.DECLINED;
            request.Comment = selectedRequest.Comment;

            _requestRepository.Update(request);
        }

        internal void AcceptRequest(ReservationRequest selectedRequest)
        {
            ReservationRequest request = selectedRequest;
            request.Status = RequestStatus.ACCEPTED;
            _requestRepository.Update(request);

            AccommodationReservation accommodationReservation = _accommodationReservationRepository.GetAll().Find(r => r.Id == selectedRequest.ReservationId);
            accommodationReservation.StartDate = selectedRequest.NewStartDate;
            accommodationReservation.EndDate = selectedRequest.NewEndDate;
            _accommodationReservationRepository.Update(accommodationReservation);
        }
    }
}
