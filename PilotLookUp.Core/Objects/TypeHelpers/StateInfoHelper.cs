using Ascon.Pilot.SDK;
using PilotLookUp.Core.Objects;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Core.Objects.TypeHelpers
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
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\stateIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
} 