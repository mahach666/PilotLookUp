using Ascon.Pilot.Bim.SDK.ModelViewer;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    // Безопасность снупа обеспечивает ObjReflection: void-методы без параметров
    // (FitToView, ClearSelection, IsolateSelection) не вызываются
    public class ModelViewerHelper : PilotObjectHelper
    {
        public ModelViewerHelper(IModelViewer obj)
        {
            _lookUpObject = obj;
            _name = "Viewer " + obj.ModelId;
            _stringId = obj.TabId.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return TypeIcons.Get("modelidicon");
        }
    }
}
