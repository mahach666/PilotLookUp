﻿using Ascon.Pilot.SDK;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class UserStateMachineHelper : PilotObjectHelper
    {
        public UserStateMachineHelper(IUserStateMachine obj)
        {
            _lookUpObject = obj;
            _name = obj.Title;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\stateMachineIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
