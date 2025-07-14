using System.Windows.Media.Imaging;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class NullHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public NullHelper()
        {
            _lookUpObject = null;
            _name = "null";
            _isLookable = false;
        }

        public override BitmapImage GetImage()
        {
            return null;
        }
    }
}
