using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IRequestedTourNotificationRepository
    {
        public IEnumerable<RequestedTourNotification> GetAll();
        public int NextId();
        public RequestedTourNotification Save(RequestedTourNotification tourNotification);
        public void Update(RequestedTourNotification tourNotification);
    }
}
