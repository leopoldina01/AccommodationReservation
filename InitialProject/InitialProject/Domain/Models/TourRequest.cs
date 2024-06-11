using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public enum TourRequestStatus
    {
        ON_HOLD = 1,
        ACCEPTED,
        DECLINED // Should be invalid but accepted - declined sounds better
    }

    public class TourRequest : ISerializable
    {
        public int Id { get; set; }
        public DateTime RequestArrivalDate { get; set; }
        public Location Location{ get; set; }
        public int LocationId { get; set; }
        public string Description { get; set; }
        public string Language  { get; set; }
        public int GuestsNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TourRequestStatus Status { get; set; }
        public int GuideId { get; set; }
        public User Guide { get; set; }
        public int UserId { get; set; }

        public TourRequest() { }

        public TourRequest(DateTime requestArrivalDate, Location location, int locationId, string description, string language, int guestsNumber, DateTime startDate, DateTime endDate, TourRequestStatus status, int guideId, User guide, int userId)
        {
            RequestArrivalDate = requestArrivalDate;
            Location = location;
            LocationId = locationId;
            Description = description;
            Language = language;
            GuestsNumber = guestsNumber;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            GuideId = guideId;
            Guide = guide;
            UserId = userId;

        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), RequestArrivalDate.ToString(), LocationId.ToString(), Description, Language, GuestsNumber.ToString(), StartDate.ToString(), EndDate.ToString(), Status.ToString(), GuideId.ToString(), UserId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            RequestArrivalDate = DateTime.Parse(values[1]);
            LocationId = int.Parse(values[2]);
            Description = values[3];
            Language = values[4];
            GuestsNumber = int.Parse(values[5]);
            StartDate = DateTime.Parse(values[6]);
            EndDate = DateTime.Parse(values[7]);
            Status = Enum.Parse<TourRequestStatus>(values[8]);
            GuideId = int.Parse(values[9]);
            UserId = int.Parse(values[10]);
        }
    }
}
