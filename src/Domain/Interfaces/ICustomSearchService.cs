using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Entities;
using PilotLookUp.Objects;
using System.Threading.Tasks;

namespace PilotLookUp.Domain.Interfaces
{
    public interface ICustomSearchService
    {
        public Task<ObjectSet> GetObjByString(string request);

        public Task<ObjectSet> GetBaseParentsOfRelations(IPilotObjectHelper objectHelper, bool findRevoked = false);

        public Task<IPilotObjectHelper> GetLastParent(IDataObject dataObject);

    }
}
