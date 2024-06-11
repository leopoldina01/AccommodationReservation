using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class ComplexTourPartService
    {
        private readonly IComplexTourRequestRepository _complexTourRequestRepository;
        private readonly IComplexTourPartRepository _complexTourPartRepository;
        private readonly TourRequestService _tourRequestService;

        public ComplexTourPartService()
        {
            _complexTourRequestRepository = Injector.CreateInstance<IComplexTourRequestRepository>();
            _complexTourPartRepository = Injector.CreateInstance<IComplexTourPartRepository>();
            _tourRequestService = new TourRequestService();
        }

        public ComplexTourPart Save(ComplexTourPart complexTourPart)
        {
            return _complexTourPartRepository.Save(complexTourPart);
        }

        public ComplexTourPart GetByRequest(TourRequest tourRequest)
        {
            return _complexTourPartRepository.GetAll().ToList().Find(p => p.TourRequestId == tourRequest.Id);
        }

        public IEnumerable<ComplexTourPart> GetAllByComplexRequest(ComplexTourRequest complexTourRequest)
        {
            var parts = _complexTourPartRepository.GetAll().Where(p => p.ComplexTourId == complexTourRequest.Id);
            foreach (var part in parts)
            {
                part.TourRequest = _tourRequestService.GetById(part.TourRequestId);
            }
            return parts;
        }

        public bool IsComplexTourPart(TourRequest tourRequest)
        {
            return this.GetByRequest(tourRequest) is not null;
        }

        public IEnumerable<ComplexTourPart> GetAll()
        {
            return _complexTourPartRepository.GetAll();
        }
    }
}
