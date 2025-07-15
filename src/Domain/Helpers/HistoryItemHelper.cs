using Ascon.Pilot.SDK.Data;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Helpers
{
    public class HistoryItemHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public HistoryItemHelper(IThemeService themeService,
            IHistoryItem obj,
            ILogger logger)
            : base(themeService, logger)
        {
            _lookUpObject = obj;
            _name = obj?.Created.ToString();
            _isLookable = true;
            _stringId = obj?.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\historyItemIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
