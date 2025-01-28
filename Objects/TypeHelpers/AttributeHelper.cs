using Ascon.Pilot.SDK;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class AttributeHelper : PilotObjectHelper
    {
        public AttributeHelper(IAttribute obj , IObjectsRepository objectsRepository) 
        {
            _lookUpObject = obj;
            _name = obj.Title;
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            throw new System.NotImplementedException();
        }
    }
}
