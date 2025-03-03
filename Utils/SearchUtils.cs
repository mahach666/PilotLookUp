using Ascon.Pilot.SDK;
using PilotLookUp.Extensions;
using PilotLookUp.Objects;
using PilotLookUp.Objects.TypeHelpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PilotLookUp.Utils
{
    static class SearchUtils
    {
        static public async Task<ObjectSet> GetObjByString(IObjectsRepository objectsRepository, string request)
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

        static public async Task<ObjectSet> GetBaseParentsOfChildren(IObjectsRepository objectsRepository, PilotObjectHelper objectHelper, bool findRevoked = false)
        {
            ObjectSet pilotObjectHelpers = new ObjectSet(null);
            ObjectSet childrenSet;

            if (objectHelper.LookUpObject is IDataObject dataObject)
            {
                childrenSet = await new Tracer(objectsRepository, null, null).Trace(dataObject.Children);
            }
            else return null;

            foreach (var child in childrenSet)
            {
                if (child is DataObjectHelper dataHelp && child.LookUpObject is IDataObject dataObj)
                {
                    if (dataHelp.IsTask)
                    {
                        if (!findRevoked) // Пропуск отозванных заданий
                        {
                            if (dataHelp.IsRevokedTask)
                            {
                                continue;
                            }
                        }
                        DataObjectHelper lastParrent = await GetLastParent(objectsRepository, dataObj);
                        if (!pilotObjectHelpers.Select(it => it.StringId).Contains(lastParrent.StringId))
                        {
                            pilotObjectHelpers.Add(lastParrent);
                        }
                    }
                }
            }
            return pilotObjectHelpers;
        }

        static public async Task<DataObjectHelper> GetLastParent(IObjectsRepository objectsRepository, IDataObject dataObject)
        {
            if (dataObject != null)
            {
                var parent = await dataObject.FindLastParrent(objectsRepository);
                var objSet = await new Tracer(objectsRepository, null, null).Trace(parent);
                return objSet.FirstOrDefault() is DataObjectHelper result ? result : null;
            }
            return null;
        }
    }
}
