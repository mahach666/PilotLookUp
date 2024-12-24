using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class StorageDataObjectHelper : PilotObjectHelper
    {
        public StorageDataObjectHelper(IStorageDataObject obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.DataObject.DisplayName;
            _isLookable = true;
        }
    }
}
