using Ascon.Pilot.SDK;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class OtherHelper : PilotObjectHelper
    {
        public OtherHelper(object obj)
        {
            _lookUpObject = obj;
            _name = obj.ToString();

            // Неизвестные объекты (включая внутренние типы клиента вроде
            // IfcNodeId или ObjectsTreeViewModel) снупаются генерически рефлексией
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return null;
        }
    }
}
