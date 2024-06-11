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
    public class ComplexTourRequestRepository : IComplexTourRequestRepository
    {

        private const string FilePath = "../../../Resources/Data/complexTourRequests.csv";
        
        private readonly Serializer<ComplexTourRequest> _serializer;

        private List<ComplexTourRequest> _complexTourRequests;

        public ComplexTourRequestRepository()
        {
            _serializer = new Serializer<ComplexTourRequest>();
            _complexTourRequests = _serializer.FromCSV(FilePath);
        }

        public void Delete(ComplexTourRequest tourRequest)
        {
            _complexTourRequests = _serializer.FromCSV(FilePath);
            ComplexTourRequest found = _complexTourRequests.Find(t => t.Id == tourRequest.Id);
            _complexTourRequests.Remove(found);
            _serializer.ToCSV(FilePath, _complexTourRequests);
        }

        public IEnumerable<ComplexTourRequest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public ComplexTourRequest GetById(int id)
        {
            _complexTourRequests = _serializer.FromCSV(FilePath);
            foreach(var req in _complexTourRequests)
            {
                if (req.Id == id)
                {
                    return req;
                }
            }
            return null;
        }

        public int NextId()
        {
            _complexTourRequests = _serializer.FromCSV(FilePath);
            if (_complexTourRequests.Count < 1)
            {
                return 1;
            }
            return _complexTourRequests.Max(t => t.Id) + 1;
        }

        public ComplexTourRequest Save(ComplexTourRequest tourRequest)
        {
            tourRequest.Id = NextId();
            _complexTourRequests = _serializer.FromCSV(FilePath);
            _complexTourRequests.Add(tourRequest);
            _serializer.ToCSV(FilePath, _complexTourRequests);
            return tourRequest;
        }

        public ComplexTourRequest Update(ComplexTourRequest tourRequest)
        {
            _complexTourRequests = _serializer.FromCSV(FilePath);
            ComplexTourRequest current = _complexTourRequests.Find(t => t.Id == tourRequest.Id);
            int index = _complexTourRequests.IndexOf(current);
            _complexTourRequests.Remove(current);
            _complexTourRequests.Insert(index, tourRequest);
            _serializer.ToCSV(FilePath, _complexTourRequests);
            return tourRequest;
        }
    }
}
