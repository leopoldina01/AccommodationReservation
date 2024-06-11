using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class AccommodationRenovation : ISerializable
    {
        public enum RenovationStatus
        {
            NOT_STARTED = 1,
            STARTED,
            FINISHED,
            DECLINED
        }

        public int Id { get; set; }
        public Accommodation Accommodation { get; set; }
        public int AccommodationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RenovationLength { get; set; }
        public string Comment { get; set; }

        public AccommodationRenovation() { }

        public AccommodationRenovation(int id, Accommodation accommodation, DateTime startDate, DateTime endDate, int renovationLenght, string comment)
        {
            Id = id;
            Accommodation = accommodation;
            AccommodationId = accommodation.Id;
            StartDate = startDate;
            EndDate = endDate;
            RenovationLength = renovationLenght;
            Comment = comment;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                StartDate.ToString(),
                EndDate.ToString(),
                RenovationLength.ToString(),
                Comment
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            StartDate = DateTime.Parse(values[2]);
            EndDate = DateTime.Parse(values[3]);
            RenovationLength = Convert.ToInt32(values[4]);
            Comment = values[5];
        }
    }
}
