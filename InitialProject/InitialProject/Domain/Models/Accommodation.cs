using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public enum AccommodationType { apartment = 1, house, cottage };
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public int LocationId { get; set; }
        public AccommodationType Type { get; set; }
        public int Capacity { get; set; }
        public int MinDaysForStay { get; set; }
        public int MinDaysBeforeCancel { get; set; }
        public int OwnerId { get; set; }
        public string SuperOwnerMark { get; set; }
        public string RenovationStatus { get; set; }

        public Accommodation()
        {
            MinDaysBeforeCancel = 1;
        }

        public Accommodation(int id, string name, Location location, AccommodationType type, int capacity, int minDaysForStay, int minDaysBeforeCancel, int ownerId, string superOwnerMark)
        {
            Id = id;
            Name = name;
            Location = location;
            LocationId = location.Id;
            Type = type;
            Capacity = capacity;
            MinDaysForStay = minDaysForStay;
            MinDaysBeforeCancel = minDaysBeforeCancel;
            OwnerId = ownerId;
            SuperOwnerMark = superOwnerMark;
            RenovationStatus = "";
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                //Location.ToString(),
                LocationId.ToString(),
                Type.ToString(),
                Capacity.ToString(),
                MinDaysForStay.ToString(),
                MinDaysBeforeCancel.ToString(),
                OwnerId.ToString(),
                SuperOwnerMark,
                RenovationStatus
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            LocationId = Convert.ToInt32(values[2]);
            switch (values[3])
            {
                case "apartment":
                    Type = AccommodationType.apartment;
                    break;
                case "house":
                    Type = AccommodationType.house;
                    break;
                case "cottage":
                    Type = AccommodationType.cottage;
                    break;
            }
            Capacity = Convert.ToInt32(values[4]);
            MinDaysForStay = Convert.ToInt32(values[5]);
            MinDaysBeforeCancel = Convert.ToInt32(values[6]);
            OwnerId = Convert.ToInt32(values[7]);
            SuperOwnerMark = values[8];
            RenovationStatus = values[9];
        }
    }


}
