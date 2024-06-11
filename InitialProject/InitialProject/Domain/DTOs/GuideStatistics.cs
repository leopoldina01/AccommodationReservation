using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.DTOs
{
    public class GuideStatistics
    {
        public string Language { get; set; }
        public int TourNumber { get; set; }
        public double AverageGrade { get; set; }

        public GuideStatistics(string language, int tourNumber, double averageGrade)
        {
            Language = language;
            TourNumber = tourNumber;
            AverageGrade = averageGrade;
        }
    }
}
