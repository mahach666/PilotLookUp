using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PilotLookUp.Model.Services
{
    public class RepoService : IRepoService
    {
        private readonly IObjectsRepository _objectsRepository;
        private readonly IObjectSetFactory _objectSetFactory;
        private readonly IValidationService _validationService;
        private readonly IPilotObjectHelperFactory _factory;

        public RepoService(IObjectsRepository objectsRepository, IObjectSetFactory objectSetFactory, IPilotObjectHelperFactory factory, IValidationService validationService)
        {
            _validationService = validationService;
            _validationService.ValidateConstructorParams(objectsRepository, objectSetFactory, factory, validationService);
            _objectsRepository = objectsRepository;
            _objectSetFactory = objectSetFactory;
            _factory = factory;
        }

        public async Task<List<ObjectSet>> GetObjInfo(IPilotObjectHelper sender)
        {
            _validationService.ValidateNotNull(sender, nameof(sender));
            var res = new List<ObjectSet>();
            foreach (var pair in sender.Reflection.KeyValuePairs)
            {
                ObjectSet newPilotObj = await new Tracer(_objectsRepository, _factory, sender, pair.Key, _objectSetFactory).Trace(pair.Value);
                res.Add(newPilotObj);
            }
            return res;
        }
        public ObjectSet GetWrapedRepo()
        {
            var pilotObjectMap = new PilotObjectMap(_objectsRepository, _factory);
            var repo = _objectSetFactory.Create(null);
            repo.Add(pilotObjectMap.Wrap(_objectsRepository));
            return repo;
        }
        public async Task<ObjectSet> GetWrapedObjs(IEnumerable<Guid> guids)
        {
            _validationService.ValidateNotNull(guids, nameof(guids));
            return await new Tracer(_objectsRepository, _factory, null, null, _objectSetFactory).Trace(guids);
        }
    }
}