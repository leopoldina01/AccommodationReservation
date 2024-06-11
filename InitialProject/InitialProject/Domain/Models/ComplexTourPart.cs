using InitialProject.Serializer;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class ComplexTourPart : ISerializable
    {
        public int Id { get; set; }
        public int TourRequestId { get; set; }
        public TourRequest TourRequest { get; set; }
        public int ComplexTourId { get; set; }
        public ComplexTourRequest ComplexTourRequest { get; set; }

        public ComplexTourPart()
        {
            
        }

        public ComplexTourPart(int tourRequestId, TourRequest tourRequest, int complexTourId, ComplexTourRequest complexTourRequest)
        {
            TourRequestId = tourRequestId;
            TourRequest = tourRequest;
            ComplexTourId = complexTourId;
            ComplexTourRequest = complexTourRequest;
        }

        public string[] ToCSV()
        {
            string[] cssValues = { Id.ToString(), TourRequestId.ToString(), ComplexTourId.ToString() };
            return cssValues;
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TourRequestId = int.Parse(values[1]);
            ComplexTourId = int.Parse(values[2]);
        }

    }
}
