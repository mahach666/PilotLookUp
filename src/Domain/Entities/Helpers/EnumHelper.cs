using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities.Helpers
{
    public class EnumHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public EnumHelper(IThemeService themeService, Enum obj, ILogger logger)
            : base(themeService, logger)
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
