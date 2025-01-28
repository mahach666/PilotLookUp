using Ascon.Pilot.SDK;
using PilotLookUp.Utils;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class TypeHelper : PilotObjectHelper
    {
        private static string TypePngPath { get; set; }

        public TypeHelper(IType obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Title;
            _isLookable = true;
            _stringId = obj.Id.ToString();
            PngControlPath(obj);
        }

        private void PngControlPath(IType obj)
        {
            if (string.IsNullOrWhiteSpace(TypePngPath))
                TypePngPath = SvgToPngConverter.SaveSvgToPng(obj.SvgIcon, obj.Name);

            _pngPath = TypePngPath;
        }
    }
}
