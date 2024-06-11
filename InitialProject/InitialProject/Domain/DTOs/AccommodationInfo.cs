using InitialProject.WPF.Views.Guest1Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.DTOs
{
    public class AccommodationInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
        public int MaxGuests { get; set; }
        public int MinReservationDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<DateTime> Dates { get; set; }
        public List<AvailableDate> AvailableDates { get; set; }

        public AccommodationInfo(int id, string name, string country, string city, string type, int maxGuests, int minReservationDays, DateTime startDate, DateTime endDate, List<DateTime> dates)
        {
            Id = id;
            Name = name;
            Country = country;
            City = city;
            Type = type;
            MaxGuests = maxGuests;
            MinReservationDays = minReservationDays;
            StartDate = startDate;
            EndDate = endDate;
            Dates = dates;
        }
    }
}
