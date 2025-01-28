using Ascon.Pilot.SDK;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class LockInfoHelper : PilotObjectHelper
    {
        public LockInfoHelper(ILockInfo value, IObjectsRepository objectsRepository)
        {
            _lookUpObject = value;
            _name = value.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            throw new System.NotImplementedException();
        }
    }
}