using Ascon.Pilot.SDK;
using System;
using System.Windows.Media.Imaging;
using PilotLookUp.Domain.Interfaces;

namespace PilotLookUp.Domain.Entities
{
    public class UserStateMachineHelper : PilotObjectHelper
    {
        public UserStateMachineHelper(IThemeService themeService, IUserStateMachine obj)
            : base(themeService)
        {
            _lookUpObject = obj;
            _name = obj?.Title;
            _isLookable = true;
            _stringId = obj?.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\stateMachineIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
