using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class Checkpoint : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public Tour Tour { get; set; }
        public int TourId { get; set; }

        public Checkpoint() { }

        public Checkpoint(int id, string name, bool active, Tour tour, int tourId)
        {
            Id = id;
            Name = name;
            Active = active;
            Tour = tour;
            TourId = tourId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, Active.ToString(), TourId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            Active = bool.Parse(values[2]);
            TourId = int.Parse(values[3]);
        }
    }
}
