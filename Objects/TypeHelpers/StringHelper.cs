using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class StringHelper : PilotObjectHelper
    {
        public StringHelper(string value, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = value;
            _name = value;
        }
    }
}
