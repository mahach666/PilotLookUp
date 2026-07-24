using Ascon.Pilot.Bim.SDK.ModelStorage;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    // Хранилищем владеет клиент (IDisposable) - Dispose при снупе заблокирован deny-list'ом
    public class ModelStorageHelper : PilotObjectHelper
    {
        public ModelStorageHelper(IModelStorage obj)
        {
            _lookUpObject = obj;
            _name = "Storage " + obj.ModelId;
            _stringId = obj.ModelId.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return TypeIcons.Get("modelidicon");
        }
    }
}
