using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using PilotLookUp.Infrastructure.Converters;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities.Helpers
{
    public class UserStateHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public UserStateHelper(
            IThemeService themeService,
            IUserState obj,
            ILogger logger)
            : base(themeService, logger)
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
