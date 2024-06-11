using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace InitialProject.Domain.Models
{
    public class Forum : ISerializable
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int LocationId { get; set; }
        public int CreatorId { get; set; }
        public Location Location { get; set; }
        public bool Utility { get; set; }

        public Forum() { }

        public Forum(int id, string status, int locationId, int creatorId)
        {
            Id = id;
            Status = status;
            LocationId = locationId;
            CreatorId = creatorId;
            Utility = false;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Status,
                LocationId.ToString(),
                CreatorId.ToString()
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Status = values[1];
            LocationId = Convert.ToInt32(values[2]);
            CreatorId = Convert.ToInt32(values[3]);
        }
    }
}
