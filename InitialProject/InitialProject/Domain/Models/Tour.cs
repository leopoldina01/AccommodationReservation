using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public enum TourStatus
    {
        NOT_STARTED = 1,
        ACTIVE,
        FINISHED,
        CANCELED
    }
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public int LocationId { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxGuests { get; set; }
        public DateTime StartTime { get; set; }
        public double Duration { get; set; }
        public string CoverImageUrl { get; set; }
        public int GuideId { get; set; }
        public TourStatus Status { get; set; }

        public Tour() { }
        public Tour(int id, string name, Location location, int locationId, string description, string language, int maxGuests, DateTime startTime, double duration, string coverImageUrl, int guideId, TourStatus status)
        {
            Id = id;
            Name = name;
            Location = location;
            LocationId = locationId;
            Description = description;
            Language = language;
            MaxGuests = maxGuests;
            StartTime = startTime;
            Duration = duration;
            CoverImageUrl = coverImageUrl;
            GuideId = guideId;
            Status = status;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, LocationId.ToString(), Description, Language, MaxGuests.ToString(), StartTime.ToString(), Duration.ToString(), CoverImageUrl, GuideId.ToString(), Status.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            LocationId = int.Parse(values[2]);
            Description = values[3];
            Language = values[4];
            MaxGuests = int.Parse(values[5]);
            StartTime = DateTime.Parse(values[6]);
            Duration = int.Parse(values[7]);
            CoverImageUrl = values[8];
            GuideId = int.Parse(values[9]);
            Status = Enum.Parse<TourStatus>(values[10]);
        }
    }
}
