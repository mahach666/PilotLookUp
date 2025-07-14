using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Utils;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class UserStateHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public UserStateHelper(IUserState obj)
        {
            _lookUpObject = obj;
            _name = obj.Title;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return SvgToPngConverter.GetBitmapImageBySvg(((IUserState)_lookUpObject).Icon);
        }
    }
}
