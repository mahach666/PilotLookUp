using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class StringHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public StringHelper(IThemeService themeService, string value)
            : base(themeService)
        {
            _lookUpObject = value;
            _name = value;
            _isLookable = false;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\stringIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
