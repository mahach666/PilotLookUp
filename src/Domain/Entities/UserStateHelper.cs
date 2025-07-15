using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Utils;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities
{
    public class UserStateHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public UserStateHelper(IThemeService themeService, IUserState obj)
            : base(themeService)
        {
            _lookUpObject = obj;
            _name = obj?.Title;
            _isLookable = true;
            _stringId = obj?.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return SvgToPngConverter.GetBitmapImageBySvg(((IUserState)_lookUpObject).Icon);
        }
    }
}
