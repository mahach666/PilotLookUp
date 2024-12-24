using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class PersonHelper : PilotObjectHelper
    {
        public PersonHelper(IPerson obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.DisplayName;
        }
    }
}
