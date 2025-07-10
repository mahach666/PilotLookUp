using Ascon.Pilot.SDK;
using PilotLookUp.Core.Objects;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Core.Objects.TypeHelpers
{
    public class RelationHelper : PilotObjectHelper
    {
        public RelationHelper(IRelation obj)
        {
            _lookUpObject = obj;
            // IRelation не имеет Name, используем Type и Id
            _name = $"Relation {obj.Type} ({obj.Id})";
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\relationIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
