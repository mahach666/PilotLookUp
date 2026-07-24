using Ascon.Pilot.Bim.SDK;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class PropertySetHelper : PilotObjectHelper
    {
        public PropertySetHelper(IPropertySet obj)
        {
            _lookUpObject = obj;
            _name = obj.Name;
            _stringId = obj.Name;
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return TypeIcons.Get("attrIcon");
        }
    }
}
