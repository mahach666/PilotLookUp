using Ascon.Pilot.Bim.SDK;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class MeshDefinitionIdHelper : PilotObjectHelper
    {
        public MeshDefinitionIdHelper(IMeshDefinitionId obj)
        {
            _lookUpObject = obj;
            _name = obj.MeshId.ToString();
            _stringId = obj.MeshId.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return TypeIcons.Get("modelidicon");
        }
    }
}
