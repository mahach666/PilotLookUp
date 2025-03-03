using Ascon.Pilot.SDK;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class OtherHelper : PilotObjectHelper
    {
        public OtherHelper(object obj, IObjectsRepository objectsRepository) 
        {
            _lookUpObject = obj;
            _name = obj.ToString();
            _isLookable = false;
        }

        public override BitmapImage GetImage()
        {
            return null;
        }
    }
}
