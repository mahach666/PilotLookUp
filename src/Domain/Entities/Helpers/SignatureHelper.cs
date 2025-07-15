using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities.Helpers
{
    internal class SignatureHelper : PilotObjectHelper, IPilotObjectHelper
    {
        internal SignatureHelper(IThemeService themeService,
            ISignature obj,
            ILogger logger)
            : base(themeService, logger)
        {
            _lookUpObject = obj;
            _name = obj?.ToString();
            _isLookable = true;
            _stringId = obj?.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\storageIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
