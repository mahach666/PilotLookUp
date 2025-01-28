using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class StringHelper : PilotObjectHelper
    {
        public StringHelper(string value, IObjectsRepository objectsRepository) 
        {
            _lookUpObject = value;
            _name = value;
            _isLookable = false;
        }
    }
}
