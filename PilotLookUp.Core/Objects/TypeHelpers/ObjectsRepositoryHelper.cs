using Ascon.Pilot.SDK;
using PilotLookUp.Core.Objects;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Core.Objects.TypeHelpers
{
    internal class ObjectsRepositoryHelper : PilotObjectHelper
    {
        public ObjectsRepositoryHelper( IObjectsRepository objectsRepository) 
        {
            _lookUpObject = objectsRepository;
            _name = "ObjectsRepository";
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\UI\databaseEnabled.png", UriKind.RelativeOrAbsolute));
        }
    }
}
