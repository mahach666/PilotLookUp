using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class DataObjectHelper : PilotObjectHelper
    {
        public DataObjectHelper(IDataObject obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.DisplayName;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }
    }
}
