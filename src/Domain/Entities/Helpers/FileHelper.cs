using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities.Helpers
{
    public class FileHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public FileHelper(IThemeService themeService, IFile value, ILogger logger)
            : base(themeService, logger)
        {
            _lookUpObject = value;
            _name = value?.Name.ToString();
            _isLookable = true;
            _stringId = value?.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\fileIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
