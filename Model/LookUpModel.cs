using Ascon.Pilot.SDK;
using PilotLookUp.Model.Utils;
using PilotLookUp.Objects;
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
    }
}