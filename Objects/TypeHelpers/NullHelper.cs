using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class NullHelper : PilotObjectHelper
    {
        public NullHelper(IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = null;
            _name = "NULL";
            _isLookable = false;
        }
    }
}
