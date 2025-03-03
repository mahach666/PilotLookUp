using Ascon.Pilot.SDK;
using System.Runtime.Remoting.Lifetime;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class LeaseHelper : PilotObjectHelper
    {
        public LeaseHelper(ILease obj, IObjectsRepository objectsRepository)
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
