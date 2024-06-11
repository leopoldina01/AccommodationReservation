using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IComplexTourRequestRepository
    {
        public IEnumerable<ComplexTourRequest> GetAll();
        public ComplexTourRequest Save(ComplexTourRequest tourRequest);
        public int NextId();
        public void Delete(ComplexTourRequest tourRequest);
        public ComplexTourRequest Update(ComplexTourRequest tourRequest);
        public ComplexTourRequest GetById(int id);
    }
}
