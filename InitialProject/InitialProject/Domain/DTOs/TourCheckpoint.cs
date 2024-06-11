using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace InitialProject.Domain.DTOs
{
    public class TourCheckpoint
    {
        public int Id { get; set; }
        public int TourId{ get; set; }
        public string TourName { get; set; }
        public TourStatus Status { get; set; }
        public int CheckpointId { get; set;}
        public string CheckpointName { get; set; }
        public int UserId { get; set; }

        public TourCheckpoint() { }

        public TourCheckpoint(int tourId, string tourName, TourStatus status, int checkpointId, string checkpointName, int userId)
        {
            TourId = tourId;
            TourName = tourName;
            Status = status;
            CheckpointId = checkpointId;
            CheckpointName = checkpointName;
            UserId = userId;
        }
    }
}
