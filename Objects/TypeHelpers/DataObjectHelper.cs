using Ascon.Pilot.SDK;
using PilotLookUp.Utils;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class DataObjectHelper : PilotObjectHelper
    {
        private static string TypePngPath { get; set; }

        public DataObjectHelper(IDataObject obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.DisplayName;
            _isLookable = true;
            _stringId = obj.Id.ToString();
            PngControlPath(obj);
        }

        private void PngControlPath(IDataObject obj)
        {
            if (string.IsNullOrWhiteSpace(TypePngPath))
                TypePngPath = SvgToPngConverter.SaveSvgToPng(obj.Type.SvgIcon, obj.Type.Name);

            _pngPath = TypePngPath;
        }
    }
}
