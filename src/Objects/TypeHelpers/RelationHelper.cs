using Ascon.Pilot.SDK;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class RelationHelper : PilotObjectHelper
    {
        public RelationHelper(IRelation obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Name is null ? obj.Id.ToString() : obj.Name;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\..\Resources\TypeIcons\relationIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
