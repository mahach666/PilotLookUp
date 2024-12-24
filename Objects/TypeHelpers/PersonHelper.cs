using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class PersonHelper : PilotObjectHelper
    {
        public PersonHelper(IPerson obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.DisplayName;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }
    }
}
