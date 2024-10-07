using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class FileHelper : PilotObjectHelper
    {
        public FileHelper(IFile obj)
        {
            LookUpObject = obj;
            Name = obj.Name;
        }
    }
}
