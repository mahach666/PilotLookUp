using Ascon.Pilot.SDK;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class UserStateMachineHelper : PilotObjectHelper
    {
        public UserStateMachineHelper(IUserStateMachine obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Title;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            throw new System.NotImplementedException();
        }
    }
}
