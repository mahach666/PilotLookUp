using Ascon.Pilot.SDK;
using PilotLookUp.Core.Objects;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Core.Objects.TypeHelpers
{
    public class TransitionHelper : PilotObjectHelper
    {
        public TransitionHelper(ITransition obj)
        {
            _lookUpObject = obj;
            _name = obj.DisplayName;
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\transitionIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
} 