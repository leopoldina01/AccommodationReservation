using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InitialProject.Domain.Models
{
    public enum NotificationStatus
    {
        READ = 1,
        UNREAD
    }
    public class TourNotification : ISerializable
    {
        public int Id{ get; set; }
        public string Name { get; set; }
        public string TextContent { get; set; }
        public NotificationStatus Status { get; set; }
        public DateTime NotificationArrivalTime { get; set; }
        public int UserId { get; set; }
        //public int GuideId { get; set; }
        public int CheckpointArrivalId { get; set; }
        public CheckpointArrival CheckpointArrival{ get; set; }

        public TourNotification(string textContent, NotificationStatus status, DateTime notificationArrivalTime, int userId, int checkpointArrivalId, string name)
        {
            Name = name;
            TextContent = textContent;
            Status = status;
            NotificationArrivalTime = notificationArrivalTime;
            UserId = userId;
            //GuideId = guideId;
            CheckpointArrivalId = checkpointArrivalId;
        }
        public TourNotification()
        {
            
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TextContent, Status.ToString(), NotificationArrivalTime.ToString(), UserId.ToString(), /*GuideId.ToString(),*/ CheckpointArrivalId.ToString(), Name };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TextContent = values[1];
            Status = Enum.Parse<NotificationStatus>(values[2]);
            NotificationArrivalTime = DateTime.Parse(values[3]);
            UserId = int.Parse(values[4]);
            //GuideId = int.Parse(values[5]);
            CheckpointArrivalId = int.Parse(values[5]);
            Name = values[6];
        }
    }
}
