using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class OtherHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public OtherHelper(IThemeService themeService, object value)
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
