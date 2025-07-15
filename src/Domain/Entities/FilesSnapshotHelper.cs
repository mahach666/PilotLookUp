using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities
{
    public class FilesSnapshotHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public FilesSnapshotHelper(IThemeService themeService, IFilesSnapshot obj)
            : base(themeService)
        {
            _lookUpObject = obj;
            _name = obj.Created.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\fileSnapshotIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
