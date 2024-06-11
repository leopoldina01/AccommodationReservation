using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.DTOs
{
    public class TourRequestYearlyStatistics
    {
        public int Year { get; set; }
        public int RequestNumber { get; set; }

        public TourRequestYearlyStatistics(int year, int requestNumber)
        {
            Year = year;
            RequestNumber = requestNumber;
        }
    }
}
