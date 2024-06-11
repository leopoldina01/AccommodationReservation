using InitialProject.Domain.Models;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IUserRepository
    {
        public User GetByUsername(string username);
        public List<User> GetAll();
        public User GetById(int id);
        public IEnumerable<string> GetGuidesNames();
        public User GetUserByName(string name);
        public User Update(User user);
    }
}
