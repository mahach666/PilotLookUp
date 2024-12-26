using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class LongHelper : PilotObjectHelper
    {
        public LongHelper(long value, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = value;
            _name = value.ToString();
            _isLookable = false;
        }
    }
}
