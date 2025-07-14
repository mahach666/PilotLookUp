using PilotLookUp.Interfaces;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class LongHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public LongHelper(long value)
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
