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
        private IObjectsRepository _objectsRepository { get; }
        private IBimModelService _bimModelService { get; }

        public RepoService(IObjectsRepository objectsRepository, IBimModelService bimModelService)
        {
            _objectsRepository = objectsRepository;
            _bimModelService = bimModelService;
        }

        public async Task<List<ObjectSet>> GetObjInfo(PilotObjectHelper sender)
        {
            var res = new List<ObjectSet>();
            foreach (var pair in sender.Reflection.KeyValuePairs)
            {
                ObjectSet newPilotObj = await new Tracer(_objectsRepository, sender, pair.Key, _bimModelService).Trace(pair.Value);
                res.Add(newPilotObj);
            }
            return res;
        }
        public ObjectSet GetWrapedRepo()
        {
            var pilotObjectMap = new PilotObjectMap(_objectsRepository);
            var repo = new ObjectSet(null) { pilotObjectMap.Wrap(_objectsRepository) };
            return repo;
        }
        public async Task<ObjectSet> GetWrapedObjs(IEnumerable<Guid> guids)
        {
            return await new Tracer(_objectsRepository, null, null, _bimModelService).Trace(guids);
        }
    }
}