using Ascon.Pilot.SDK;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class TransitionHelper : PilotObjectHelper
    {
        public TransitionHelper(ITransition obj, IObjectsRepository objectsRepository) 
        {
            _lookUpObject = obj;
            _name = obj.DisplayName;
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            throw new System.NotImplementedException();
        }
    }
}
