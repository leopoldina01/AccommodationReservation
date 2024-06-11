using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.DTOs
{
    public class ComplexAndRegularTourRequest
    {
        public int ComplexTourRequestId { get; set; }
        public TourRequestStatus Status { get; set; }
        public TourRequest TourRequest{ get; set; }

        public ComplexAndRegularTourRequest()
        {
           
        }

        public ComplexAndRegularTourRequest(int complexTourRequestId, TourRequestStatus tourRequestStatus, TourRequest tourRequest)
        {
            ComplexTourRequestId = complexTourRequestId;
            Status = tourRequestStatus;
            TourRequest = tourRequest;
        }
    }
}
