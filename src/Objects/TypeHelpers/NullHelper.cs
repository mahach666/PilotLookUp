﻿using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class NullHelper : PilotObjectHelper
    {
        public NullHelper()
        {
            _lookUpObject = null;
            _name = "NULL";
            _isLookable = false;
        }

        public override BitmapImage GetImage()
        {
            return null;
        }
    }
}
