using Ascon.Pilot.SDK;
using PilotLookUp.Objects;
using PilotLookUp.Objects.TypeHelpers;
using System.Threading.Tasks;

namespace PilotLookUp.Interfaces
{
    public interface ICustomSearchService
    {
        public Task<ObjectSet> GetObjByString(string request);

        public Task<ObjectSet> GetBaseParentsOfRelations(PilotObjectHelper objectHelper, bool findRevoked = false);

        public Task<DataObjectHelper> GetLastParent(IDataObject dataObject);

    }
}
