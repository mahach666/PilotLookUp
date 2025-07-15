using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Utils;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities
{
    public class AttributeHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public AttributeHelper(IThemeService themeService,
            IAttribute obj,
            ILogger logger)
            : base(themeService, logger)
        {
            _lookUpObject = obj;
            _name = obj.Title;
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\attrIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
