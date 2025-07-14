using Ascon.Pilot.SDK;
using PilotLookUp.Objects;
using PilotLookUp.Objects.TypeHelpers;
using System.Threading.Tasks;

namespace PilotLookUp.Interfaces
{
    public interface ICustomSearchService
    {
        public Task<ObjectSet> GetObjByString(string request);

        public Task<ObjectSet> GetBaseParentsOfRelations(IPilotObjectHelper objectHelper, bool findRevoked = false);

        public Task<IPilotObjectHelper> GetLastParent(IDataObject dataObject);

    }
}
