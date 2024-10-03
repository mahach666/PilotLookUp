using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal abstract class TypeHelper
    {
        public string TypeFullName { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public object LookUpObject { get; set; }
    }
}
