﻿using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class PersonHelper : PilotObjectHelper
    {
        public PersonHelper(IPerson obj)
        {
            LookUpObject = obj;
            Name = obj.DisplayName;
        }
    }
}
