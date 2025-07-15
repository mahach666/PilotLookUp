using PilotLookUp.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class LongHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public LongHelper(IThemeService themeService, long value)
            : base(themeService)
        {
            _lookUpObject = value;
            _name = value.ToString();
            _isLookable = false;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\intIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
