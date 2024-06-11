using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class MostVisitedTourService
    {
        private readonly CheckpointArrivalService _checkpointArrivalService;
        private readonly TourService _tourService;

        public MostVisitedTourService()
        {
            _checkpointArrivalService = new CheckpointArrivalService();
            _tourService = new TourService();
        }

        public Tour GetAllTimeMostVisitedTour()
        {
            return _tourService.GetPastTours().OrderByDescending(t => GetAttendance(t)).FirstOrDefault();
        }

        private int? GetAttendance(Tour tour)
        {
            return _checkpointArrivalService.GetAll().Where(c => c.Reservation.TourId == tour.Id).Sum(c => c.Reservation.NumberOfPeople);
        }

        public IEnumerable<int> GetYearsThatHaveTours()
        {
            return _tourService.GetPastTours().Select(t => t.StartTime.Year).ToHashSet().OrderBy(y => y);
        }

        public Tour GetMostVisitedTourByYear(int year)
        {
            return _tourService.GetPastTours().Where(t => t.StartTime.Year == year).OrderByDescending(t => GetAttendance(t)).FirstOrDefault();
        }
    }
}
