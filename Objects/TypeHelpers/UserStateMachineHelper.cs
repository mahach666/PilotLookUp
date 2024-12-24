using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class UserStateMachineHelper : PilotObjectHelper
    {
        public UserStateMachineHelper(IUserStateMachine obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Title;
        }
    }
}
