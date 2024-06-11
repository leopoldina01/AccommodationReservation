using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IComplexTourPartRepository
    {
        public IEnumerable<ComplexTourPart> GetAll();
        public ComplexTourPart Save(ComplexTourPart tourPart);
        public int NextId();
        public void Delete(ComplexTourPart tourPart);
        public ComplexTourPart Update(ComplexTourPart tourPart);
        public ComplexTourPart GetById(int id);
    }
}
