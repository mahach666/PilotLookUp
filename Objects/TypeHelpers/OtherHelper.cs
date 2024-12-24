using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class OtherHelper : PilotObjectHelper
    {
        public OtherHelper(object obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.ToString();
            _isLookable = false;
        }
    }
}
