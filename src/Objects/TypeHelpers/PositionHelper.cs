using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class PositionHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public PositionHelper(IThemeService themeService, IPosition obj)
            : base(themeService)
        {
            _lookUpObject = obj;
            _name = obj?.ToString();
            _isLookable = true;
            _stringId = obj?.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\organisationUnitIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
