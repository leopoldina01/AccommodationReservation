using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class ReportedComments : ISerializable
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public int ReporterId { get; set; }

        public ReportedComments() { }
        
        public ReportedComments(int id, int commentId, int reporterId)
        {
            Id = id;
            CommentId = commentId;
            ReporterId = reporterId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), CommentId.ToString(), ReporterId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            CommentId = Convert.ToInt32(values[1]);
            ReporterId = Convert.ToInt32(values[2]);
        }
    }
}
