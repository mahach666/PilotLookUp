using Ascon.Pilot.SDK;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class StateInfoHelper : PilotObjectHelper
    {
        public StateInfoHelper(IStateInfo obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.State.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\stateIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
