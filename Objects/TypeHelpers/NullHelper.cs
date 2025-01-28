using Ascon.Pilot.SDK;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class NullHelper : PilotObjectHelper
    {
        public NullHelper(IObjectsRepository objectsRepository)
        {
            _lookUpObject = null;
            _name = "NULL";
            _isLookable = false;
        }

        public override BitmapImage GetImage()
        {
            throw new System.NotImplementedException();
        }
    }
}
