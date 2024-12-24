using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class DataObjectHelper : PilotObjectHelper
    {
        public DataObjectHelper(IDataObject obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.DisplayName;
            _isLookable = true;
        }
    }
}
