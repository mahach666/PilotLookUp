using Ascon.Pilot.SDK;
using PilotLookUp.Extensions;
using PilotLookUp.Objects;
using PilotLookUp.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PilotLookUp.Model.Utils
{
   static class SearchUtils
    {
        static public async Task<ObjectSet> ByString(IObjectsRepository objectsRepository, string request)
        {
            var tracer = new Tracer(objectsRepository, null, null);
            if (Guid.TryParse(request, out var id))
            {
                var res = await tracer.Trace(await objectsRepository.GetObjByGuid(id));
                return res;
            }
            else if (int.TryParse(request, out var intId))
            {
                var res = new ObjectSet(null);
                var person = objectsRepository.GetPerson(intId);
                var orgUnit = objectsRepository.GetOrganisationUnit(intId);
                var iType = objectsRepository.GetType(intId);
                if (person != null) { res.AddRange(await tracer.Trace(person)); }
                if (orgUnit != null) { res.AddRange(await tracer.Trace(orgUnit)); }
                if (iType != null) { res.AddRange(await tracer.Trace(iType)); }
                var distSet = new ObjectSet(null);
                distSet.AddRange(res.Distinct());
                return distSet;
            }
            return null;
        }
    }
}
