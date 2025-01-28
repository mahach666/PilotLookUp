using Ascon.Pilot.SDK;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class AccessHelper : PilotObjectHelper
    {
        public AccessHelper(IAccess obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.AccessLevel.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            throw new System.NotImplementedException();
        }
    }
}
