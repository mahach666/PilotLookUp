using System;
using System.Windows.Media.Imaging;
using PilotLookUp.Domain.Interfaces;

namespace PilotLookUp.Domain.Entities
{
    public class DateTimeHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public DateTimeHelper(IThemeService themeService, DateTime obj)
            : base(themeService)
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
