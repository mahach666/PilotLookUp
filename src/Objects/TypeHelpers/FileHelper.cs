using Ascon.Pilot.SDK;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class FileHelper : PilotObjectHelper
    {
        public FileHelper(IFile obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.Name;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\..\Resources\TypeIcons\fileIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
