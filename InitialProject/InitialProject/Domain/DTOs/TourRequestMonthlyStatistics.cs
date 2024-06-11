using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.DTOs
{
    public class TourRequestMonthlyStatistics
    {
        public string Month { get; set; }
        public int RequestNumber { get; set; }

        public TourRequestMonthlyStatistics(string month, int requestNumber)
        {
            Month = month;
            RequestNumber = requestNumber;
        }
    }
}
