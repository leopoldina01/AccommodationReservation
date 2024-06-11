using InitialProject.Domain.Models;
using System;

namespace InitialProject.Domain.DTOs
{
    public class ComplexTourInfo
    {
        public Location StartLocation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ComplexTourRequestId { get; set; }

        public ComplexTourInfo(Location startLocation, DateTime startDate, DateTime endDate, int complexTourRequestId)
        {
            StartLocation = startLocation;
            StartDate = startDate;
            EndDate = endDate;
            ComplexTourRequestId = complexTourRequestId;
        }
    }
}
