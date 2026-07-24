using Ascon.Pilot.Bim.SDK;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class MeshInstanceHelper : PilotObjectHelper
    {
        public MeshInstanceHelper(IMeshInstance obj)
        {
            _lookUpObject = obj;
            _name = "Mesh " + obj.MeshDefinitionId.MeshId;
            _stringId = obj.MeshDefinitionId.MeshId.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return TypeIcons.Get("modelidicon");
        }
    }
}
