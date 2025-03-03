using Ascon.Pilot.SDK;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class SignatureRequestHelper : PilotObjectHelper
    {
        public SignatureRequestHelper(ISignatureRequest obj)
        {
            _lookUpObject = obj;
            _name = obj.Id.ToString();
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\..\Resources\TypeIcons\signatureIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
