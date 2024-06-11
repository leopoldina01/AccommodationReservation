using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public enum RequestStatus
    {
        ON_HOLD = 1,
        ACCEPTED,
        DECLINED
    }
    
    public class ReservationRequest : ISerializable
    {
        public int Id { get; set; }
        public DateTime NewStartDate { get; set; }
        public DateTime NewEndDate { get; set; }
        public RequestStatus Status { get; set; }
        public AccommodationReservation Reservation { get; set; }
        public int ReservationId { get; set; }
        public string IsAvailable { get; set; }
        public string Comment { get; set; }

        public ReservationRequest() { }

        public ReservationRequest(int id, DateTime newStartDate, DateTime newEndDate, RequestStatus status, AccommodationReservation reservation)
        {
            Id = id;
            NewStartDate = newStartDate;
            NewEndDate = newEndDate;
            Status = status;
            Reservation = reservation;
            ReservationId = reservation.Id;
            IsAvailable = "";
            Comment = "";
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                NewStartDate.ToString(),
                NewEndDate.ToString(),
                Status.ToString(),
                ReservationId.ToString(),
                IsAvailable,
                Comment
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            NewStartDate = DateTime.Parse(values[1]);
            NewEndDate = DateTime.Parse(values[2]);
            switch (values[3])
            {
                case "ON_HOLD":
                    Status = RequestStatus.ON_HOLD;
                    break;
                case "ACCEPTED":
                    Status = RequestStatus.ACCEPTED;
                    break;
                case "DECLINED":
                    Status = RequestStatus.DECLINED;
                    break;

            }
            ReservationId = Convert.ToInt32(values[4]);
            IsAvailable = values[5];
            Comment = values[6];
        }
    }
}
