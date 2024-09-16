using Ascon.Pilot.SDK;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Model
{
    internal class LookUpModel
    {
        private List<IDataObject> _dataObjects { get; }

        public LookUpModel(List<IDataObject> dataObjects)
        {
            _dataObjects = dataObjects;
        }

        public List<IDataObject> SelectionDataObjects => _dataObjects;

        public ObjReflection GetInfo(IDataObject dataObject)
        {
            return new ObjReflection(dataObject);
        }
    }
}