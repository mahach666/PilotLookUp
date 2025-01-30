﻿using Ascon.Pilot.SDK;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class AccessRecordHelper : PilotObjectHelper
    {
        public AccessRecordHelper(IAccessRecord obj, IObjectsRepository objectsRepository) 
        {
            _lookUpObject = obj;
            _name = objectsRepository.GetOrganisationUnit(obj.OrgUnitId).Title;
            _isLookable = true;            
        }

        public override BitmapImage GetImage()
        {
            return null;
        }
    }
}
