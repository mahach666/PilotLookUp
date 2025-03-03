using Ascon.Pilot.SDK;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class FilesSnapshotHelper : PilotObjectHelper
    {
        public FilesSnapshotHelper(IFilesSnapshot obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Created.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\..\Resources\TypeIcons\fileSnapshotIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
