using System.Windows.Media.Imaging;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class BoolHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public BoolHelper(IThemeService themeService, bool value)
            : base(themeService)
        {
            _lookUpObject = value;
            _name = value.ToString();
            _isLookable = false;
        }

        public override BitmapImage GetImage()
        {
            return null;
        }
    }
}
