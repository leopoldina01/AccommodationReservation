using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class ComplexTourRequest : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public TourRequestStatus Status { get; set; }
        public int FirstPartId { get; set; }

        public ComplexTourRequest() { }

        public ComplexTourRequest(int userId, TourRequestStatus status, int firstPartId)
        {
            UserId = userId;
            Status = status;
            FirstPartId = firstPartId;
        }
        public string[] ToCSV()
        {
            string[] cssValues = {Id.ToString(), UserId.ToString(), Status.ToString(), FirstPartId.ToString()};
            return cssValues;
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            UserId = int.Parse(values[1]);
            Status = Enum.Parse<TourRequestStatus>(values[2]);
            FirstPartId = int.Parse(values[3]);
        }

    }
}
