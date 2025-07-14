using System.Windows.Media.Imaging;
using PilotLookUp.Interfaces;
using Ascon.Pilot.SDK.Data;
using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class SignatureRequestHelper : PilotObjectHelper, IPilotObjectHelper
    {
        internal SignatureRequestHelper(IThemeService themeService, ISignatureRequest obj)
            : base(themeService)
        {
            _lookUpObject = obj;
            _name = obj?.ToString();
            _isLookable = true;
            _stringId = obj?.ToString();
        }

        public override BitmapImage GetImage()
        {
            return null;
        }
    }
}
