using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class AccommodationNotification : ISerializable
    {
        public int Id { get; set; }
        public string TextContent { get; set; }
        public bool Seen { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int ReceiverId { get; set; }
        public string NotificationInfo { get; set; }

        public AccommodationNotification() { }

        public AccommodationNotification(string textContent, bool seen, int senderId, int receiverId)
        {
            TextContent = textContent;
            Seen = seen;
            SenderId = senderId;
            ReceiverId = receiverId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TextContent, Seen.ToString(), SenderId.ToString(), ReceiverId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TextContent = values[1];
            Seen = bool.Parse(values[2]);
            SenderId = int.Parse(values[3]);
            ReceiverId = int.Parse(values[4]);
        }
    }

}
