using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class PersonHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public PersonHelper(IThemeService themeService, IPerson obj)
            : base(themeService)
        {
            _lookUpObject = obj;
            _name = obj?.DisplayName;
            _isLookable = true;
            _stringId = obj?.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\personIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
