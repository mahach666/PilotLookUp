using Ascon.Pilot.SDK;
using PilotLookUp.Core.Objects;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Core.Objects.TypeHelpers
{
    public class TypeHelper : PilotObjectHelper
    {
        public TypeHelper(IType obj)
        {
            _lookUpObject = obj;
            _name = obj.Title;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }

                public override BitmapImage GetImage()
        {
            // TODO: Inject ISvgToPngConverter dependency
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\typeIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
