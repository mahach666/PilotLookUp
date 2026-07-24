using Ascon.Pilot.Bim.SDK;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class ModelElementHelper : PilotObjectHelper
    {
        public ModelElementHelper(IModelElement obj)
        {
            _lookUpObject = obj;
            _name = string.IsNullOrEmpty(obj.Name) ? obj.Id.ElementId.ToString() : obj.Name;
            _stringId = obj.Id.ElementId.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return TypeIcons.Get("modelidicon");
        }
    }
}
