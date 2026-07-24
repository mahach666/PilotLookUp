using Ascon.Pilot.Bim.SDK;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    // Снуп разворачивает Vertices/Indices полностью - для больших мешей это много строк,
    // но IMeshDefinition достижим только явной загрузкой (LoadMeshDefinition), не автоматически
    public class MeshDefinitionHelper : PilotObjectHelper
    {
        public MeshDefinitionHelper(IMeshDefinition obj)
        {
            _lookUpObject = obj;
            _name = "MeshDefinition";
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return TypeIcons.Get("modelidicon");
        }
    }
}
