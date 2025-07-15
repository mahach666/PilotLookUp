using PilotLookUp.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class BoolHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public BoolHelper(IThemeService themeService, bool value)
            : base(themeService)
        {
            _lookUpObject = value;
            _name = value.ToString();
            _isLookable = false;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\boolIcon.png", UriKind.RelativeOrAbsolute));      
        }
    }
}
