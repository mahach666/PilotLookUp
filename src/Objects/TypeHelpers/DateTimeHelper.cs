using System;
using System.Windows.Media.Imaging;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class DateTimeHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public DateTimeHelper(DateTime value)
        {
            _lookUpObject = value;
            _name = value.ToString("g");
            _isLookable = false;
        }

        public override BitmapImage GetImage()
        {
            return null;
        }
    }
}
