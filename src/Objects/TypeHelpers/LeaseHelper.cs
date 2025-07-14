using PilotLookUp.Interfaces;
using System.Runtime.Remoting.Lifetime;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class LeaseHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public LeaseHelper(ILease obj)
        {
            _lookUpObject = obj;
            _name = obj.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return null;
        }
    }
}
