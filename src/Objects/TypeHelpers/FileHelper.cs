using System;
using System.Windows.Media.Imaging;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class FileHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public FileHelper(object value)
        {
            _lookUpObject = value;
            _name = value?.ToString();
            _isLookable = false;
        }

        public override BitmapImage GetImage()
        {
            return null;
        }
    }
}
