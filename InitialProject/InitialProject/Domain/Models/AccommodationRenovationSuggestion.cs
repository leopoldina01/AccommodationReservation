using System;
using System.Collections.Generic;
using System.Linq;
using InitialProject.Serializer;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class AccommodationRenovationSuggestion : ISerializable
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int LevelOfUrgency { get; set; }
        public int ReservationId { get; set; }
        public int OwnerId { get; set; }
        public int SuggesterId { get; set; }

        public AccommodationRenovationSuggestion() { }

        public AccommodationRenovationSuggestion(int id, string comment, int levelOfUrgency, int reservationId, int ownerId, int suggesterId)
        {
            Id = id;
            Comment = comment;
            LevelOfUrgency = levelOfUrgency;
            ReservationId = reservationId;
            OwnerId = ownerId;
            SuggesterId = suggesterId;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Comment,
                LevelOfUrgency.ToString(),
                ReservationId.ToString(),
                OwnerId.ToString(),
                SuggesterId.ToString(),
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Comment = values[1];
            LevelOfUrgency = Convert.ToInt32(values[2]);
            ReservationId = Convert.ToInt32(values[3]);
            OwnerId = Convert.ToInt32(values[4]);
            SuggesterId = Convert.ToInt32(values[5]);
        }
    }
}
