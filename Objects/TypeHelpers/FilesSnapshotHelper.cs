﻿using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class FilesSnapshotHelper : PilotObjectHelper
    {
        public FilesSnapshotHelper(IFilesSnapshot obj)
        {
            LookUpObject = obj;
            Name = obj.Created.ToString();
        }
    }
}
