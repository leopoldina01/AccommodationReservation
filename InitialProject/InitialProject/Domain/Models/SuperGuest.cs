using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;

namespace InitialProject.Domain.Models
{
    public class SuperGuest : ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public User User { get; set; }
        public int BonusPoints { get; set; }
        public DateTime ExpirationDate { get; set; }

        public SuperGuest() { }

        public SuperGuest(int id, int guestId, int bonusPoints, DateTime expirationDate)
        {
            Id = id;
            GuestId = guestId;
            BonusPoints = bonusPoints;
            ExpirationDate = expirationDate;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(), BonusPoints.ToString(), ExpirationDate.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            BonusPoints = Convert.ToInt32(values[2]);
            ExpirationDate = Convert.ToDateTime(values[3]);
        }
    }
}
