using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class SuperGuestRepository : ISuperGuestRepository
    {
        private const string FilePath = "../../../Resources/Data/superGuests.csv";

        private readonly Serializer<SuperGuest> _serializer;

        private List<SuperGuest> _superGuests;

        public SuperGuestRepository()
        {
            _serializer = new Serializer<SuperGuest>();
            _superGuests = _serializer.FromCSV(FilePath);
        }

        public List<SuperGuest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public SuperGuest Save(int guestId, int bonusPoints, DateTime expirationDate)
        {
            int id = NextId();

            SuperGuest superGuest = new SuperGuest(id, guestId, bonusPoints, expirationDate);

            _superGuests.Add(superGuest);
            _serializer.ToCSV(FilePath, _superGuests);
            return superGuest;
        }

        public int NextId()
        {
            _superGuests = _serializer.FromCSV(FilePath);
            if (_superGuests.Count < 1)
            {
                return 1;
            }

            return _superGuests.Max(c => c.Id) + 1;
        }

        public void Remove(int guestId)
        {
            _superGuests = _serializer.FromCSV(FilePath);
            _superGuests.Remove(_superGuests.Find(x => x.GuestId == guestId));
            _serializer.ToCSV(FilePath, _superGuests);
        }

        public SuperGuest Update(int guestId, int bonusPoints, DateTime expirationDate)
        {
            var _superGuests = _serializer.FromCSV(FilePath);
            var current = _superGuests.Find(x => x.GuestId == guestId);
            int index = _superGuests.IndexOf(current);
            _superGuests.Remove(current);
            var newSuperGuest = new SuperGuest(current.Id, guestId, bonusPoints, expirationDate);
            _superGuests.Insert(index, newSuperGuest);
            _serializer.ToCSV(FilePath, _superGuests);
            return newSuperGuest;
        }
    }
}
