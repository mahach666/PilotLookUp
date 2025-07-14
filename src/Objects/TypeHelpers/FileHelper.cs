using System;
using System.Windows.Media.Imaging;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class FileHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public FileHelper(IThemeService themeService, object value)
            : base(themeService)
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
