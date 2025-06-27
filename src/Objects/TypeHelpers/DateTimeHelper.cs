using Ascon.Pilot.SDK;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class DateTimeHelper : PilotObjectHelper
    {
        public DateTimeHelper(DateTime obj)
        {
            _lookUpObject = obj;
            _name = obj.ToString();
            _isLookable = false;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\dateTimeIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
