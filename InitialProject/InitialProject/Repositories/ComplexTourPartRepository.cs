using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class ComplexTourPartRepository : IComplexTourPartRepository
    {
        private const string FilePath = "../../../Resources/Data/complexTourParts.csv";

        private readonly Serializer<ComplexTourPart> _serializer;

        private List<ComplexTourPart> _complexTourParts;

        public ComplexTourPartRepository()
        {
            _serializer = new Serializer<ComplexTourPart>();
            _complexTourParts = new List<ComplexTourPart>();
        }

        public void Delete(ComplexTourPart tourPart)
        {
            _complexTourParts = _serializer.FromCSV(FilePath);
            ComplexTourPart found = _complexTourParts.Find(t => t.Id == tourPart.Id);
            _complexTourParts.Remove(found);
            _serializer.ToCSV(FilePath, _complexTourParts);
        }

        public IEnumerable<ComplexTourPart> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public ComplexTourPart GetById(int id)
        {
            _complexTourParts = _serializer.FromCSV(FilePath);
            foreach (var part in _complexTourParts)
            {
                if (part.Id == id)
                {
                    return part;
                }
            }
            return null;
        }

        public int NextId()
        {
            _complexTourParts = _serializer.FromCSV(FilePath);
            if (_complexTourParts.Count < 1)
            {
                return 1;
            }
            return _complexTourParts.Max(t => t.Id) + 1;
        }

        public ComplexTourPart Save(ComplexTourPart tourPart)
        {
            tourPart.Id = NextId();
            _complexTourParts = _serializer.FromCSV(FilePath);
            _complexTourParts.Add(tourPart);
            _serializer.ToCSV(FilePath, _complexTourParts);
            return tourPart;
        }

        public ComplexTourPart Update(ComplexTourPart tourPart)
        {
            _complexTourParts = _serializer.FromCSV(FilePath);
            ComplexTourPart current = _complexTourParts.Find(t => t.Id == tourPart.Id);
            int index = _complexTourParts.IndexOf(current);
            _complexTourParts.Remove(current);
            _complexTourParts.Insert(index, tourPart);
            _serializer.ToCSV(FilePath, _complexTourParts);
            return tourPart;
        }
    }
}
