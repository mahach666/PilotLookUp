using Ascon.Pilot.Bim.SDK;
using System;
using System.Linq;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class ModelElementIdHelper : PilotObjectHelper
    {
        public ModelElementIdHelper(IModelElementId obj)
        {
            _lookUpObject = obj;

            var objType = obj.GetType();
            var property = objType.GetProperties().FirstOrDefault(p=>p.Name == "Title");

            _name = obj.ToString();
            _isLookable = true;
            _stringId = obj.ElementId.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\modelidicon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
