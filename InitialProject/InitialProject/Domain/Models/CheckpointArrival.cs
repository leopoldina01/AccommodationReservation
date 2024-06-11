using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace InitialProject.Domain.Models
{
    public class CheckpointArrival : ISerializable
    {
        public int Id { get; set; }
        public Checkpoint Checkpoint { get; set; }
        public int CheckpointId { get; set; }
        public TourReservation Reservation { get; set; }
        public int ReservationId { get; set; }

        public CheckpointArrival()
        {

        }

        public CheckpointArrival(int id, int checkpointId, int reservationId)
        {
            Id = id;
            CheckpointId = checkpointId;
            ReservationId = reservationId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), CheckpointId.ToString(), ReservationId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            CheckpointId = int.Parse(values[1]);
            ReservationId = int.Parse(values[2]);
        }
    }
}
