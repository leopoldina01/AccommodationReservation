using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationNotificationRepository
    {
        public List<AccommodationNotification> GetAll();
        public int NextId();
        public AccommodationNotification Save(AccommodationNotification notification);
        public void Update(AccommodationNotification updatedNotification);
    }
}
