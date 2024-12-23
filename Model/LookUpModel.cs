using Ascon.Pilot.SDK;
using PilotLookUp.Model.Utils;
using PilotLookUp.Objects;
using System.Collections;
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


        public async Task DataGridSelector(PilotObjectHelper sender ,object obj)
        {
            if (obj == null) return;
            new Tracer().Trace(_objectsRepository, sender, obj);
        }

        public async Task<Dictionary<string, List<PilotObjectHelper>>> Info(PilotObjectHelper sender)
        {
            var res = new Dictionary<string, List<PilotObjectHelper>>();

            foreach (var pair in sender.Reflection.KeyValuePairs)
            {
                List<PilotObjectHelper> newPilotObj = await new Tracer().Trace(_objectsRepository, sender, pair.Value);
                res.Add(pair.Key, newPilotObj);
            }

            return res;
        }
    }
}