using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Utils;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities
{
    public class DateTimeHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public DateTimeHelper(IThemeService themeService, DateTime obj , ILogger logger)
            : base(themeService, logger)
        {
            _lookUpObject = obj;
            _name = obj.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\dateTimeIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
