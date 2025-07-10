using Ascon.Pilot.SDK;
using PilotLookUp.Core.Objects;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Core.Objects.TypeHelpers
{
    public class OtherHelper : PilotObjectHelper
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
