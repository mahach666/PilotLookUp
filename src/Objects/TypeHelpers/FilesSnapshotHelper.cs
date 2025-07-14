using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class FilesSnapshotHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public FilesSnapshotHelper(IFilesSnapshot obj)
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
