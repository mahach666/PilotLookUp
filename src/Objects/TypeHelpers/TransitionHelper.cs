using System.Windows.Media.Imaging;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class TransitionHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public TransitionHelper(object value)
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
