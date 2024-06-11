using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows;
using System.Xml.Linq;

namespace InitialProject.Domain.Models
{
    public class SuperGuide : ISerializable
    {
        public int Id { get; set; }
        public int GuideId { get; set; }
        public User Guide { get; set; }
        public string Language { get; set; }

        public SuperGuide()
        {
            
        }

        public SuperGuide(int id, int guideId, string language)
        {
            Id = id;
            GuideId = guideId;
            Language = language;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            GuideId = int.Parse(values[1]);
            Language = values[2];
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuideId.ToString(), Language };
            return csvValues;
        }
    }
}
