using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Data;
using PilotLookUp.Interfaces;
using System;
using System.Windows.Media.Imaging;

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
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\storageIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
