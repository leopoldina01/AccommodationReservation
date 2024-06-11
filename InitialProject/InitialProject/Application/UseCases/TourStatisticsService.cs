using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class TourStatisticsService
    {
        private readonly CheckpointArrivalService _checkpointArrivalService;
        public TourStatisticsService()
        {
            _checkpointArrivalService = new CheckpointArrivalService();
        }

        public int? GetNumberOfGusetsInAgeRange(Tour tour, int youngest, int oldest)
        {
            return _checkpointArrivalService.GetAllByTour(tour).Where(c => c.Reservation.Age > youngest && c.Reservation.Age < oldest).Select(c => c.Reservation.NumberOfPeople).Sum();
        }
    }
}
