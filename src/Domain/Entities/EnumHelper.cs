using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities
{
    public class EnumHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public EnumHelper(IThemeService themeService, Enum obj)
            : base(themeService)
        {
            _lookUpObject = obj;
            _name = obj.ToString();
            _isLookable = false;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\enumIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
