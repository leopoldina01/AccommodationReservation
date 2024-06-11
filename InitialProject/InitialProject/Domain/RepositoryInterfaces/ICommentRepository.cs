using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ICommentRepository
    {
        public List<Comment> GetAll();
        public Comment Save(Comment comment);
        public int NextId();
        public void Delete(Comment comment);
        public Comment Update(Comment comment);
        public List<Comment> GetByUser(User user);
    }
}
