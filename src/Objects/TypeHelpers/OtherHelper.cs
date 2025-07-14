using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class OtherHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public OtherHelper(object obj)
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
