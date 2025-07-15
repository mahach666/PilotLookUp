using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Data;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Helpers
{
    internal class ReportItemHelper : PilotObjectHelper, IPilotObjectHelper
    {
        internal ReportItemHelper(IThemeService themeService, IReportItem obj, ILogger logger)
            : base(themeService, logger)
        {
            _lookUpObject = obj;
            _name = obj?.Name;
            _isLookable = true;
            _stringId = obj?.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\reportIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
