using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class RelationHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public RelationHelper(IThemeService themeService, IRelation obj)
            : base(themeService)
        {
            _lookUpObject = obj;
            _name = obj?.Id.ToString();
            _isLookable = true;
            _stringId = obj?.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\relationIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
