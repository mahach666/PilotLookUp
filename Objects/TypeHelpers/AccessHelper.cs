using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class AccessHelper : PilotObjectHelper
    {
        public AccessHelper( IAccess obj, IObjectsRepository objectsRepository) : base (objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.AccessLevel.ToString();
            _isLookable = true;
        }
    }
}
