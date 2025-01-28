using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class StorageDataObjectHelper : PilotObjectHelper
    {
        public StorageDataObjectHelper(IStorageDataObject obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.DataObject.DisplayName;
            _isLookable = true;
        }
    }
}
