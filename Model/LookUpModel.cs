using Ascon.Pilot.SDK;
using PilotLookUp.Objects;
using PilotLookUp.Utils;
using PilotLookUp.ViewBuilders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PilotLookUp.Model
{
    internal class LookUpModel
    {
        private ObjectSet _dataObjects { get; }
        public IObjectsRepository ObjectsRepository { get; }

        public LookUpModel(ObjectSet dataObjects, IObjectsRepository objectsRepository)
        {
            _dataObjects = dataObjects;
            ObjectsRepository = objectsRepository;
        }

        public ObjectSet SelectionDataObjects => _dataObjects;


        public void DataGridSelector(ObjectSet obj)
        {
            if (obj == null) return;
            new LookSeleсtion(obj, ObjectsRepository);
        }

        public async Task<List<ObjectSet>> Info(PilotObjectHelper sender)
        {
            var res = new List<ObjectSet>();

            foreach (var pair in sender.Reflection.KeyValuePairs)
            {
                ObjectSet newPilotObj = await new Tracer(ObjectsRepository, sender, pair.Key).Trace(pair.Value);
                res.Add(newPilotObj);
            }

            return res;
        }
    }
}