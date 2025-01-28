using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class UserStateHelper : PilotObjectHelper
    {
        public UserStateHelper(IUserState obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Title;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }
    }
}
