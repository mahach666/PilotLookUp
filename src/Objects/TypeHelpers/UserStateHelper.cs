using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Utils;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class UserStateHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public UserStateHelper(IThemeService themeService, IUserState obj)
            : base(themeService)
        {
            _lookUpObject = obj;
            _name = obj?.Name;
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return SvgToPngConverter.GetBitmapImageBySvg(((IUserState)_lookUpObject).Icon);
        }
    }
}
