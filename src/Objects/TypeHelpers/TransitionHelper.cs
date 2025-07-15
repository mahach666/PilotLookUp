using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Data;
using PilotLookUp.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class TransitionHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public TransitionHelper(IThemeService themeService, ITransition obj)
            : base(themeService)
        {
            _lookUpObject = obj;
            _name = obj?.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\transitionIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
