using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class ReportedCommentsService
    {
        private readonly IReportedCommentRepository _reportedCommentRepository;

        public ReportedCommentsService()
        {
            _reportedCommentRepository = Injector.CreateInstance<IReportedCommentRepository>();
        }

        public ReportedComments Save(int commentId, int reporterId)
        {
            return _reportedCommentRepository.Save(new ReportedComments(-1, commentId, reporterId));
        }

        public List<ReportedComments> GetAll()
        {
            return _reportedCommentRepository.GetAll();
        }

        public ReportedComments GetByOwnerId(int ownerId, int selectedCommentId)
        {
            return GetAll().Find(rc => rc.CommentId == selectedCommentId && rc.ReporterId == ownerId);
        }
    }
}
