﻿using Ascon.Pilot.SDK;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class PositionHelper : PilotObjectHelper
    {
        public PositionHelper(IPosition obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = objectsRepository.GetOrganisationUnit(obj.Position).Title;
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return null;
        }
    }
}
