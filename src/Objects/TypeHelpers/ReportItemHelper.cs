using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Data;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class ReportItemHelper : PilotObjectHelper
    {
        public ReportItemHelper(IReportItem reportItem)
        {
            _lookUpObject = reportItem;
            _name = reportItem.Name;
            _isLookable = true;
            _stringId = reportItem.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\UI\reportIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
