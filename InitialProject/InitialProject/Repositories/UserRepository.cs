using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Repositories
{
    public class UserRepository : IUserRepository
    {
        private const string FilePath = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> _serializer;

        private List<User> _users;

        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(FilePath);
        }

        public User GetByUsername(string username)
        {
            _users = _serializer.FromCSV(FilePath);
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public List<User> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public User GetById(int id)
        {
            _users = _serializer.FromCSV(FilePath);
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public User Update(User user)
        {
            _users = _serializer.FromCSV(FilePath);
            User current = _users.Find(t => t.Id == user.Id);
            int index = _users.IndexOf(current);
            _users.Remove(current);
            _users.Insert(index, user);
            _serializer.ToCSV(FilePath, _users);
            return user;
        }

        public IEnumerable<string> GetGuidesNames()
        {
            _users = _serializer.FromCSV(FilePath);
            List<string> names = new List<string>();
            foreach(var user in _users)
            {
                if(user.Role == UserRole.GUIDE)
                {
                    names.Add(user.Username);
                }
            }
            
            return names;
        }

        public User GetUserByName(string name)
        {
            _users = _serializer.FromCSV(FilePath);
            foreach(var user in _users)
            {
                if(user.Username == name)
                {
                    return user;
                }
            }
            return null;
        }
    }
}
