using Ascon.Pilot.SDK;
using PilotLookUp.Core.Objects;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Core.Objects.TypeHelpers
{
    public class SignatureRequestHelper : PilotObjectHelper
    {
        public SignatureRequestHelper(ISignatureRequest obj)
        {
            _lookUpObject = obj;
            // ISignatureRequest не имеет DisplayName, используем Role или Id
            _name = !string.IsNullOrEmpty(obj.Role) ? obj.Role : $"SignatureRequest {obj.Id}";
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\signatureIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
} 