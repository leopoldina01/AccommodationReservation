using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService()
        {
            _userRepository = Injector.CreateInstance<IUserRepository>();
        }

        public User FindOwnerById(int ownerId)
        {
            User owner = _userRepository.GetById(ownerId);

            return owner;
        }

        public User GetById(int id)
        {
            return _userRepository.GetAll().FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<string> GetAllGuidesNames()
        {
            return _userRepository.GetGuidesNames();
        }

        public User GetUserByName(string name)
        {
            return _userRepository.GetUserByName(name);
        }
    }
}
