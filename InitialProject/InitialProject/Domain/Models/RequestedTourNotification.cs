using InitialProject.Serializer;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class RequestedTourNotification : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public NotificationStatus Status { get; set; }
        public int TourId { get; set; }
        public string TextContent { get; set; }

        public RequestedTourNotification()
        {
            
        }

        public RequestedTourNotification(string name, NotificationStatus status, int tourId, string textContent)
        {
            Name = name;
            Status = status;
            TourId = tourId;
            TextContent = textContent;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, Status.ToString(), TourId.ToString(), TextContent};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            Status = Enum.Parse<NotificationStatus>(values[2]);
            TourId = int.Parse(values[3]);
            TextContent = values[4];
        }
    }
}
