using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InitialProject.Application.UseCases
{
    public class ForumService
    {
        private readonly IForumRepository _forumRepository;
        private readonly LocationService _locationService;
        private readonly CommentService _commentService;
        private readonly AccommodationService _accommodationService;
        private readonly UserLocationService _userLocationService;

        public ForumService()
        {
            _forumRepository = Injector.CreateInstance<IForumRepository>();
            _locationService = new LocationService();
            _commentService = new CommentService();
            _accommodationService = new AccommodationService();
            _userLocationService = new UserLocationService();
        }

        public Forum Save(string status, int locationId, int creatorId, string comment)
        {
            if  (GetByLocationId(locationId) != null)
            {
                Forum forum = GetByLocationId(locationId);
                if (forum.Status.Equals("Closed"))
                {
                    forum.Status = status;
                    forum.CreatorId = creatorId;
                    _forumRepository.Update(forum);
                }
                _commentService.Save(forum.Id, comment, creatorId);
                return forum;
            }
            else
            {
                Forum forum = _forumRepository.Save(status, locationId, creatorId);
                _commentService.Save(forum.Id, comment, creatorId);
                return forum;
            }
        }

        private Forum GetByLocationId(int locationId)
        {
            return _forumRepository.GetAll().Find(x => x.LocationId == locationId);
        }

        public IEnumerable<Forum> GetAll()
        {
            var forums = _forumRepository.GetAll();
            foreach(Forum forum in forums)
            {
                forum.Location = _locationService.GetLocationById(forum.LocationId);
                forum.Utility = CalculateUtility(forum).Utility;
            }
            return forums;
        }

        public List<Forum> GetForumsForOwner(User owner)
        {
            List<Forum> forumsForOwner = new List<Forum>();

            IEnumerable<Accommodation> ownersAccommodations = _accommodationService.GetByOwnerId(owner.Id);

            List<Forum> allForums = _forumRepository.GetAll();

            foreach (var forum in allForums)
            {
                if (ownersAccommodations.FirstOrDefault(oa => oa.LocationId == forum.LocationId) != null)
                {
                    forum.Location = ownersAccommodations.FirstOrDefault(oa => oa.LocationId == forum.LocationId).Location;

                    forumsForOwner.Add(CalculateUtility(forum));
                }
            }

            return forumsForOwner;
        }

        public Forum CalculateUtility(Forum forum)
        {
            Forum updatedForum = forum;

            List<Comment> comments = _commentService.GetAllByForumId(forum.Id);

            List<Comment> ownersComments = comments.FindAll(c => c.User.Role == UserRole.OWNER || c.User.Role == UserRole.SUPER_OWNER);

            List<Comment> guestsComments = comments.FindAll(c => c.User.Role == UserRole.GUEST1);

            List<Comment> guestsOnLocation = new List<Comment>();

            foreach(var comment in guestsComments)
            {
                if (_userLocationService.WasUserOnThisLocation(comment.UserId, forum.LocationId, -1))
                {
                    guestsOnLocation.Add(comment);
                }
            }

            if (ownersComments.Count >= 10 && guestsOnLocation.Count >= 20)
            {
                updatedForum.Utility = true;
            }

            return updatedForum;
        }

        public void Close(Forum forum)
        {
            forum.Status = "Closed";
            _forumRepository.Update(forum);
        }
    }
}
