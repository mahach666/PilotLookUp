using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Helpers
{
    public class UserStateMachineHelper : PilotObjectHelper
    {
        public UserStateMachineHelper(
            IThemeService themeService,
            IUserStateMachine obj,
            ILogger logger)
            : base(themeService, logger)
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
