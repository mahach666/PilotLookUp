using Ascon.Pilot.SDK;
using PilotLookUp.Utils;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class TypeHelper : PilotObjectHelper
    {
        public TypeHelper(IType obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Title;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
           return SvgToPngConverter.GetBitmapImageBySvg(((IType)_lookUpObject).SvgIcon);
        }
    }
}
