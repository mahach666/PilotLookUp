using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class StateInfoHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public StateInfoHelper(IStateInfo obj)
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
