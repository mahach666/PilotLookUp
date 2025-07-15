using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Utils;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities
{
    public class StringHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public StringHelper(
            IThemeService themeService,
            string value,
            ILogger logger)
            : base(themeService, logger)
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
