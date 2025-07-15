using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Helpers
{
    public class LongHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public LongHelper(IThemeService themeService,
            long value,
            ILogger logger)
            : base(themeService, logger)
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
