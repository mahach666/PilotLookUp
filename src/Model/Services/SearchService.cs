using Ascon.Pilot.SDK;
using PilotLookUp.Extensions;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Domain.Entities;
using PilotLookUp.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;
using PilotLookUp.ViewModel;
using System.Collections.Generic;

namespace PilotLookUp.Model.Services
{
    public class SearchService : ICustomSearchService
    {
        private readonly IObjectsRepository _objectsRepository;
        private readonly IObjectSetFactory _objectSetFactory;
        private readonly IPilotObjectHelperFactory _factory;

        public SearchService(IObjectsRepository objectsRepository, IObjectSetFactory objectSetFactory, IPilotObjectHelperFactory factory)
        {
            _objectsRepository = objectsRepository;
            _objectSetFactory = objectSetFactory;
            _factory = factory;
        }

        public async Task<ObjectSet> GetObjByString(string request)
        {
            var tracer = new Tracer(_objectsRepository, _factory, null, null, _objectSetFactory);
            if (Guid.TryParse(request, out var id))
            {
                var res = await tracer.Trace(await _objectsRepository.GetObjByGuid(id));
                return res;
            }
            else if (int.TryParse(request, out var intId))
            {
                var res = _objectSetFactory.Create(null);
                var person = _objectsRepository.GetPerson(intId);
                var orgUnit = _objectsRepository.GetOrganisationUnit(intId);
                var iType = _objectsRepository.GetType(intId);
                if (person != null) { res.AddRange(await tracer.Trace(person)); }
                if (orgUnit != null) { res.AddRange(await tracer.Trace(orgUnit)); }
                if (iType != null) { res.AddRange(await tracer.Trace(iType)); }
                var distSet = _objectSetFactory.Create(null);
                distSet.AddRange(res.Distinct());
                return distSet;
            }
            return null;
        }

        public async Task<ObjectSet> GetBaseParentsOfRelations(IPilotObjectHelper objectHelper, bool findRevoked = false)
        {
            ObjectSet pilotObjectHelpers = _objectSetFactory.Create(null);
            ObjectSet childrenSet;

            if (objectHelper.LookUpObject is IDataObject dataObject)
            {
                var listId = dataObject.Relations.Where(it => it.Type == ObjectRelationType.TaskAttachments).Select(fd => fd.TargetId).ToList();
                childrenSet = await new Tracer(_objectsRepository, _factory, null, null, _objectSetFactory).Trace(listId);
            }
            else return null;

            foreach (var child in childrenSet)
            {
                if (child is IPilotObjectHelper dataHelp && dataHelp.LookUpObject is IDataObject dataObj)
                {
                    // Проверяем, есть ли свойство IsTask через интерфейс
                    var isTaskProp = dataHelp.GetType().GetProperty("IsTask");
                    var isTask = isTaskProp != null && (bool)isTaskProp.GetValue(dataHelp);
                    if (isTask)
                    {
                        if (!findRevoked)
                        {
                            var isRevokedProp = dataHelp.GetType().GetProperty("IsRevokedTask");
                            var isRevoked = isRevokedProp != null && (bool)isRevokedProp.GetValue(dataHelp);
                            if (isRevoked)
                            {
                                continue;
                            }
                        }
                        var lastParrent = await GetLastParent(dataObj);
                        if (!pilotObjectHelpers.Select(it => it.StringId).Contains(lastParrent.StringId))
                        {
                            pilotObjectHelpers.Add(lastParrent);
                        }
                    }
                }
            }
            return pilotObjectHelpers;
        }

        public async Task<IPilotObjectHelper> GetLastParent(IDataObject dataObject)
        {
            if (dataObject != null)
            {
                var parent = await dataObject.FindLastParrent(_objectsRepository);
                var objSet = await new Tracer(_objectsRepository, _factory, null, null, _objectSetFactory).Trace(parent);
                return objSet.FirstOrDefault();
            }
            return null;
        }

        public async Task<List<SearchResVM>> SearchAndMapVMsAsync(string request, INavigationService navigationService, ITabService tabService, IObjectSetFactory objectSetFactory, IValidationService validationService)
        {
            var objectSet = await GetObjByString(request);
            var res = new List<SearchResVM>();
            if (objectSet != null)
            {
                foreach (var item in objectSet)
                {
                    var vm = new SearchResVM(navigationService, tabService, item, objectSetFactory, validationService);
                    res.Add(vm);
                }
            }
            return res;
        }
    }
}
