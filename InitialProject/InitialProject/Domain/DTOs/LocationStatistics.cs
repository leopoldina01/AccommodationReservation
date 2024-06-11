using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.DTOs
{
    public class LocationStatistics
    {
        public Location Location { get; set; }
        public double NumberOfRequests { get; set; }

        public LocationStatistics()
        {
            
        }

        public LocationStatistics(Location location, double numberOfRequests)
        {
            Location = location;
            NumberOfRequests = numberOfRequests;
        }
    }
}
