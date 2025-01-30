using Ascon.Pilot.SDK;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
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
            return null;
        }
    }
}
