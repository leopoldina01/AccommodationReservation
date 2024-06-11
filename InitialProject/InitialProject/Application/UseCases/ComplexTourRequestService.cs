using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class ComplexTourRequestService
    {
        private readonly IComplexTourRequestRepository _complexTourRequestRepository;
        private readonly IComplexTourPartRepository _complexTourPartRepository;
        private readonly ComplexTourPartService _complexTourPartService;

        public ComplexTourRequestService()
        {
            _complexTourRequestRepository = Injector.CreateInstance<IComplexTourRequestRepository>();
            _complexTourPartRepository = Injector.CreateInstance<IComplexTourPartRepository>();
            _complexTourPartService = new ComplexTourPartService();
        }

        public ComplexTourRequest Save(ComplexTourRequest complexTourRequest)
        {
            return _complexTourRequestRepository.Save(complexTourRequest);
        }

        public IEnumerable<ComplexTourRequest> GetAll()
        {
            return _complexTourRequestRepository.GetAll();
        }

        public IEnumerable<TourRequest> GetAllParts(ComplexTourRequest complexTourRequest)
        {
            List<TourRequest> requests = new List<TourRequest>();
            foreach (var part in _complexTourPartService.GetAllByComplexRequest(complexTourRequest))
            {
                requests.Add(part.TourRequest);
            }
            return requests;
        }

        public ComplexTourRequest GetById(int id)
        {
            return _complexTourRequestRepository.GetById(id);
        }


        public ComplexTourRequest Update(ComplexTourRequest complexTourRequest)
        {
            return _complexTourRequestRepository.Update(complexTourRequest);
        }
    }
}
