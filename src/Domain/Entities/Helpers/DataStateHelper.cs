using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities.Helpers
{
    public class DataStateHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public DataStateHelper(IThemeService themeService, DataState obj, ILogger logger)
            : base(themeService, logger)
        {
            _lookUpObject = obj;
            _name = obj.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\stateIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
