using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class NullHelper : PilotObjectHelper
    {
        public NullHelper(IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = null;
            _name = "NULL";
        }
    }
}
