using Ascon.Pilot.SDK;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class LongHelper : PilotObjectHelper
    {
        public LongHelper(long value, IObjectsRepository objectsRepository)
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
