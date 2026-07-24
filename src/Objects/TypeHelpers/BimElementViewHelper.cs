using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    // Элемент модели со свойствами (снапшот, загруженный при выделении)
    public class BimElementViewHelper : PilotObjectHelper
    {
        public BimElementViewHelper(BimElementView obj)
        {
            _lookUpObject = obj;
            _name = string.IsNullOrEmpty(obj.Element?.Name)
                ? obj.Id.ElementId.ToString()
                : obj.Element.Name;
            _stringId = obj.Id.ElementId.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return TypeIcons.Get("modelidicon");
        }
    }
}
