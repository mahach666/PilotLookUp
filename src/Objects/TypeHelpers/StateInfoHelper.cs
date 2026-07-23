using Ascon.Pilot.SDK;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class StateInfoHelper : PilotObjectHelper
    {
        public StateInfoHelper(IStateInfo obj)
        {
            _lookUpObject = obj;
            _name = obj.State.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return TypeIcons.Get("stateIcon");
        }
    }
}
