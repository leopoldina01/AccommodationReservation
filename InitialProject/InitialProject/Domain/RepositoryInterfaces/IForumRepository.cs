using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IForumRepository
    {
        public List<Forum> GetAll();
        public Forum Save(string status, int locationId, int creatorId);
        public int NextId();
        public void Update(Forum forum);
    }
}
