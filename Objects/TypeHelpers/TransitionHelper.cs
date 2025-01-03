﻿using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class TransitionHelper : PilotObjectHelper
    {
        public TransitionHelper(ITransition obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.DisplayName;
            _isLookable = true;
        }
    }
}
