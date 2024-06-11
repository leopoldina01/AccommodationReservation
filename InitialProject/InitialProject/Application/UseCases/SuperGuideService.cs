using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System.Collections.Generic;

namespace InitialProject.Application.UseCases
{
    public class SuperGuideService
    {
        private readonly ISuperGuideRepository _superGuideRepository;
        public SuperGuideService()
        {
            _superGuideRepository = Injector.CreateInstance<ISuperGuideRepository>();
        }

        public IEnumerable<SuperGuide> getAll()
        {
            return _superGuideRepository.GetAll();
        }

        public SuperGuide Create(int guideId, string language)
        {
            return _superGuideRepository.Create(guideId, language);
        }

        public SuperGuide GetByGuideAndLanguage(User guide, string language)
        {
            return _superGuideRepository.GetAll().Find(s => s.GuideId == guide.Id && s.Language.Equals(language));
        }
    }
}
