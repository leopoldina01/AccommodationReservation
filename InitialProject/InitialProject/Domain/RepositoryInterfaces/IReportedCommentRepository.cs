using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IReportedCommentRepository
    {
        public List<ReportedComments> GetAll();
        public ReportedComments Save(ReportedComments comment);
        public int NextId();
        public void Delete(ReportedComments reportedComment);
        public ReportedComments Update(ReportedComments comment);
    }
}
