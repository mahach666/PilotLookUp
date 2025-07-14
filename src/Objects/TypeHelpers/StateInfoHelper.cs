using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class StateInfoHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public StateInfoHelper(IThemeService themeService, IStateInfo obj)
            : base(themeService)
        {
            _lookUpObject = obj;
            _name = obj?.State.ToString();
            _isLookable = true;
            _stringId = obj?.State.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\stateIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
