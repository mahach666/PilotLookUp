using Ascon.Pilot.SDK;
using PilotLookUp.Model.Utils;
using PilotLookUp.Objects;
using PilotLookUp.ViewBuilders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PilotLookUp.Model
{
    internal class LookUpModel
    {
        private List<PilotObjectHelper> _dataObjects { get; }
        private IObjectsRepository _objectsRepository { get; }

        public LookUpModel(List<PilotObjectHelper> dataObjects, IObjectsRepository objectsRepository)
        {
            _dataObjects = dataObjects;
            _objectsRepository = objectsRepository;
        }

        public List<PilotObjectHelper> SelectionDataObjects => _dataObjects;


        public async Task DataGridSelector(ObjectSet obj)
        {
            if (obj == null) return;
            new LookSeleсtion(obj, _objectsRepository);
        }

        public async Task<List<ObjectSet>> Info(PilotObjectHelper sender)
        {
            var res = new List<ObjectSet>();

            foreach (var pair in sender.Reflection.KeyValuePairs)
            {
                ObjectSet newPilotObj = await new Tracer(_objectsRepository, sender, pair.Key).Trace(pair.Value);
                res.Add(newPilotObj);
            }

            return res;
        }
    }
}