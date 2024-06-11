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
    public class ForumRepository : IForumRepository
    {
        private const string FilePath = "../../../Resources/Data/forums.csv";

        private readonly Serializer<Forum> _serializer;

        private List<Forum> _forums;

        public ForumRepository()
        {
            _serializer = new Serializer<Forum>();
            _forums = _serializer.FromCSV(FilePath);
        }

        public List<Forum> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Forum Save(string status, int locationId, int creatorId)
        {
            int id = NextId();

            Forum forum = new Forum(id, status, locationId, creatorId);
            _forums.Add(forum);
            _serializer.ToCSV(FilePath, _forums);
            return forum;
        }

        public int NextId()
        {
            _forums = _serializer.FromCSV(FilePath);
            if (_forums.Count < 1)
            {
                return 1;
            }

            return _forums.Max(c => c.Id) + 1;
        }

        public void Update(Forum forum)
        {
            _forums = _serializer.FromCSV(FilePath);
            foreach (var f in _forums)
            {
                if (f.Id == forum.Id)
                {
                    f.Status = forum.Status;
                    f.LocationId = forum.LocationId;
                    f.CreatorId = forum.CreatorId;
                }
            }
            _serializer.ToCSV(FilePath, _forums);
        }
    }
}
