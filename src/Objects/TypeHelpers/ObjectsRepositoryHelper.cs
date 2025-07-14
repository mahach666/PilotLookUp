using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class ObjectsRepositoryHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public ObjectsRepositoryHelper(IObjectsRepository objectsRepository)
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
