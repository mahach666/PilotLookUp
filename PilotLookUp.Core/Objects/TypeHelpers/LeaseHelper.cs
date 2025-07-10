using Ascon.Pilot.SDK;
using PilotLookUp.Core.Objects;
using System.Runtime.Remoting.Lifetime;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Core.Objects.TypeHelpers
{
    public class LeaseHelper : PilotObjectHelper
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
