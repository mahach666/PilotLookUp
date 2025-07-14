using Ascon.Pilot.SDK;
using System;
using System.Windows.Media.Imaging;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class UserStateMachineHelper : PilotObjectHelper
    {
        public UserStateMachineHelper(IThemeService themeService, IUserStateMachine obj)
            : base(themeService)
        {
            _lookUpObject = obj;
            _name = obj?.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\stateMachineIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
