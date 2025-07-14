using System.Windows.Media.Imaging;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class SignatureRequestHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public SignatureRequestHelper(object value)
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
