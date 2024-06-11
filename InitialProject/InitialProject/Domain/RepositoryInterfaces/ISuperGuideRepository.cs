using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ISuperGuideRepository
    {
        public List<SuperGuide> GetAll();
        public SuperGuide Save(SuperGuide superGuide);
        public int NextId();
        public void Delete(SuperGuide superGuide);
        public SuperGuide Update(SuperGuide superGuide);
        public SuperGuide Create(int guideId, string language);
        public SuperGuide GetById(int id);
    }
}
