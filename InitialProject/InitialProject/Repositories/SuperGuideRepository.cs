using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Repositories
{
    public class SuperGuideRepository : ISuperGuideRepository
    {
        private const string FilePath = "../../../Resources/Data/superGuides.csv";

        private readonly Serializer<SuperGuide> _serializer;

        private List<SuperGuide> _guides;

        public SuperGuideRepository()
        {
            _serializer = new Serializer<SuperGuide>();
            _guides = _serializer.FromCSV(FilePath);
        }
        public SuperGuide Create(int guideId, string language)
        {
            _guides = _serializer.FromCSV(FilePath);
            SuperGuide newSuperGuide = new SuperGuide(NextId(), guideId, language);
            _guides.Add(newSuperGuide);
            _serializer.ToCSV(FilePath, _guides);
            return newSuperGuide;
        }

        public void Delete(SuperGuide superGuide)
        {
            _guides = _serializer.FromCSV(FilePath);
            SuperGuide found = _guides.Find(s => s.Id == superGuide.Id);
            _guides.Remove(found);
            _serializer.ToCSV(FilePath, _guides);
        }

        public List<SuperGuide> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public SuperGuide GetById(int id)
        {
            _guides = _serializer.FromCSV(FilePath);
            foreach (var guides in _guides)
            {
                if (guides.Id == id)
                {
                    return guides;
                }
            }
            return null;
        }

        public int NextId()
        {
            _guides = _serializer.FromCSV(FilePath);
            if (_guides.Count < 1)
            {
                return 1;
            }
            return _guides.Max(t => t.Id) + 1;
        }

        public SuperGuide Save(SuperGuide superGuide)
        {
            superGuide.Id = NextId();
            _guides = _serializer.FromCSV(FilePath);
            _guides.Add(superGuide);
            _serializer.ToCSV(FilePath, _guides);
            return superGuide;
        }

        public SuperGuide Update(SuperGuide superGuide)
        {
            _guides = _serializer.FromCSV(FilePath);
            SuperGuide current = _guides.Find(t => t.Id == superGuide.Id);
            int index = _guides.IndexOf(current);
            _guides.Remove(current);
            _guides.Insert(index, superGuide);
            _serializer.ToCSV(FilePath, _guides);
            return superGuide;
        }
    }
}
