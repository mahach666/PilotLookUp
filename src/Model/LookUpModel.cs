using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.Utils;
using PilotLookUp.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PilotLookUp.Model
{
    public class LookUpModel
    {
        private IObjectsRepository _objectsRepository { get; }

        public LookUpModel(IObjectsRepository objectsRepository)
        {
            _objectsRepository = objectsRepository;
        }

        public async Task<List<ObjectSet>> GetObjInfo(PilotObjectHelper sender)
        {
            var res = new List<ObjectSet>();
            foreach (var pair in sender.Reflection.KeyValuePairs)
            {
                ObjectSet newPilotObj = await new Tracer(_objectsRepository, sender, pair.Key).Trace(pair.Value);
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

        public async Task<ICastomTree> FillChild(ICastomTree lastParrent)
            => await TreeViewUtils.FillChild(_objectsRepository, lastParrent);
    }
}