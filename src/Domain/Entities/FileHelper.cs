using System;
using System.Windows.Media.Imaging;
using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;

namespace PilotLookUp.Domain.Entities
{
    public class FileHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public FileHelper(IThemeService themeService, IFile value)
            : base(themeService)
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
