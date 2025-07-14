using System.Windows.Media.Imaging;
using PilotLookUp.Interfaces;
using Ascon.Pilot.SDK.Data;
using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class TransitionHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public TransitionHelper(IThemeService themeService, ITransition obj)
            : base(themeService)
        {
            _lookUpObject = obj;
            _name = obj?.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return null;
        }
    }
}
