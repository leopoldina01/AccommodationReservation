using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class ReportedCommentRepository : IReportedCommentRepository
    {
        private const string FilePath = "../../../Resources/Data/reportedComments.csv";

        private readonly Serializer<ReportedComments> _serializer;

        private List<ReportedComments> _comments;

        public ReportedCommentRepository()
        {
            _serializer = new Serializer<ReportedComments>();
            _comments = _serializer.FromCSV(FilePath);
        }

        public List<ReportedComments> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public ReportedComments Save(ReportedComments comment)
        {
            comment.Id = NextId();
            _comments = _serializer.FromCSV(FilePath);
            _comments.Add(comment);
            _serializer.ToCSV(FilePath, _comments);
            return comment;
        }

        public int NextId()
        {
            _comments = _serializer.FromCSV(FilePath);
            if (_comments.Count < 1)
            {
                return 1;
            }
            return _comments.Max(c => c.Id) + 1;
        }

        public void Delete(ReportedComments comment)
        {
            _comments = _serializer.FromCSV(FilePath);
            ReportedComments founded = _comments.Find(c => c.Id == comment.Id);
            _comments.Remove(founded);
            _serializer.ToCSV(FilePath, _comments);
        }

        public ReportedComments Update(ReportedComments comment)
        {
            _comments = _serializer.FromCSV(FilePath);
            ReportedComments current = _comments.Find(c => c.Id == comment.Id);
            int index = _comments.IndexOf(current);
            _comments.Remove(current);
            _comments.Insert(index, comment);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _comments);
            return comment;
        }
    }
}
