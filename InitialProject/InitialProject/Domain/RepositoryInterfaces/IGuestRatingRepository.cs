using InitialProject.Domain.Models;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IGuestRatingRepository
    {
        public List<GuestRating> GetAll();
        public GuestRating Save(string cleanliness, string followingTheRules, string comment, int theOneWhoIsRatedId, int raterId, int reservationId);
        public int NextId();
        public GuestRating GetByReservationId(int id);
    }
}
