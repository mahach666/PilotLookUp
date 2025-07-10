using Ascon.Pilot.SDK;
using PilotLookUp.Core.Objects;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Core.Objects.TypeHelpers
{
    public class FileHelper : PilotObjectHelper
    {
        public FileHelper(IFile obj)
        {
            _lookUpObject = obj;
            _name = obj.Name;
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\fileIcon.png", UriKind.RelativeOrAbsolute));
        }
    }

    public class FilesSnapshotHelper : PilotObjectHelper
    {
        public FilesSnapshotHelper(IFilesSnapshot obj)
        {
            _lookUpObject = obj;
            _name = obj.Reason ?? $"FilesSnapshot ({obj.Created:yyyy-MM-dd HH:mm:ss})";
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\fileSnapshotIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
} 
