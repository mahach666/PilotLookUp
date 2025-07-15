using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Helpers
{
    internal class BoolHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public BoolHelper(IThemeService themeService,
            bool value,
            ILogger logger)
            : base(themeService, logger)
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
