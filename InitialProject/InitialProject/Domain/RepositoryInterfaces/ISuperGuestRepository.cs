using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ISuperGuestRepository
    {
        public List<SuperGuest> GetAll();
        public SuperGuest Save(int guestId, int bonusPoints, DateTime expirationDate);
        public int NextId();
        public void Remove(int superGuestId);
        public SuperGuest Update(int guestId, int bonusPoints, DateTime expirationDate);
    }
}
