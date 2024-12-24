using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class UserStateHelper : PilotObjectHelper
    {
        public UserStateHelper(IUserState obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Title;
        }
    }
}
