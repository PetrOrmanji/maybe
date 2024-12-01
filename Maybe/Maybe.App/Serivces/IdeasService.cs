using EnsureThat;
using Maybe.Common.Utils;
using Maybe.Domain.Data;

namespace Maybe.App.Serivces
{
    public class IdeasService
    {
        private readonly string _path;
        private readonly IdeasDocument _ideasDoc;

        public IdeasService(string path) 
        {
            EnsureArg.IsNotNullOrWhiteSpace(path, nameof(path));
            
            _path = path;
            _ideasDoc = Serialization.Deserialize<IdeasDocument>(_path);
        }

        public List<Idea> GetAll(bool? publicated = null)
        {
            return publicated == null
                ? _ideasDoc.Ideas
                : publicated.Value
                    ? _ideasDoc.Ideas.Where(x => x.Publicated != null).ToList()
                    : _ideasDoc.Ideas.Where(x => x.Publicated == null).ToList();
        }

        public Idea? Get(Guid id)
        {
            return _ideasDoc.Ideas.FirstOrDefault(x => x.Id == id);
        }

        public Idea? GetNotPublicatedIdea()
        {
            return _ideasDoc?.Ideas?.FirstOrDefault(x => x.Publicated == null);
        }

        public bool AddFromFile(List<Idea> ideas)
        {
            if(ideas is null || !ideas.Any())
            {
                return false;
            }

            if (_ideasDoc.Ideas is null)
            {
                _ideasDoc.Ideas = new List<Idea>();
            }

            _ideasDoc.Ideas.AddRange(ideas);
            return true;
        }

        public void Remove(Guid id)
        {
            var idea = Get(id);

            if(idea != null)
                _ideasDoc.Ideas.Remove(idea);
        }

        public void RemoveAll()
        {
            _ideasDoc.Ideas?.Clear();
        }

        public int Count(bool? publicated = null)
        {
            return publicated == null
                ? _ideasDoc.Ideas?.Count() ?? 0
                : publicated.Value
                    ? _ideasDoc.Ideas?.Count(x => x.Publicated != null) ?? 0
                    : _ideasDoc.Ideas?.Count(x => x.Publicated == null) ?? 0;
        }

        public void SaveChanges()
        {
            Serialization.Serialize(_ideasDoc, _path);
        }
    }
}
