using Ascon.Pilot.SDK;
using PilotLookUp.Extensions;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.Objects.TypeHelpers;
using PilotLookUp.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PilotLookUp.Model.Services
{
    public class SearchService : ICustomSearchService
    {
        private IObjectsRepository _objectsRepository { get; }

        public SearchService(IObjectsRepository objectsRepository)
        {
            _objectsRepository = objectsRepository;
        }

        public async Task<ObjectSet> GetObjByString(string request)
        {
            var tracer = new Tracer(_objectsRepository, null, null);
            if (Guid.TryParse(request, out var id))
            {
                var res = await tracer.Trace(await _objectsRepository.GetObjByGuid(id));
                return res;
            }
            else if (int.TryParse(request, out var intId))
            {
                var res = new ObjectSet(null);
                var person = _objectsRepository.GetPerson(intId);
                var orgUnit = _objectsRepository.GetOrganisationUnit(intId);
                var iType = _objectsRepository.GetType(intId);
                if (person != null) { res.AddRange(await tracer.Trace(person)); }
                if (orgUnit != null) { res.AddRange(await tracer.Trace(orgUnit)); }
                if (iType != null) { res.AddRange(await tracer.Trace(iType)); }
                var distSet = new ObjectSet(null);
                distSet.AddRange(res.Distinct());
                return distSet;
            }
            return null;
        }

        public async Task<ObjectSet> GetBaseParentsOfRelations(PilotObjectHelper objectHelper, bool findRevoked = false)
        {
            ObjectSet pilotObjectHelpers = new ObjectSet(null);
            ObjectSet childrenSet;

            if (objectHelper.LookUpObject is IDataObject dataObject)
            {
                var listId = dataObject.Relations.Where(it => it.Type == ObjectRelationType.TaskAttachments).Select(fd => fd.TargetId).ToList();
                childrenSet = await new Tracer(_objectsRepository, null, null).Trace(listId);
            }
            else return null;

            foreach (var child in childrenSet)
            {
                if (child is DataObjectHelper dataHelp && child.LookUpObject is IDataObject dataObj)
                {
                    if (dataHelp.IsTask)
                    {
                        if (!findRevoked)
                        {
                            if (dataHelp.IsRevokedTask)
                            {
                                continue;
                            }
                        }
                        DataObjectHelper lastParrent = await GetLastParent(dataObj);
                        if (!pilotObjectHelpers.Select(it => it.StringId).Contains(lastParrent.StringId))
                        {
                            pilotObjectHelpers.Add(lastParrent);
                        }
                    }
                }
            }
            return pilotObjectHelpers;
        }

        public async Task<DataObjectHelper> GetLastParent(IDataObject dataObject)
        {
            if (dataObject != null)
            {
                var parent = await dataObject.FindLastParrent(_objectsRepository);
                var objSet = await new Tracer(_objectsRepository, null, null).Trace(parent);
                return objSet.FirstOrDefault() is DataObjectHelper result ? result : null;
            }
            return null;
        }
    }
}
