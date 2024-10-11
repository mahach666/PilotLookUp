using PilotLookUp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Objects
{
    internal abstract class PilotObjectHelper
    {
        public string Name { get; set; }
        public object LookUpObject { get; set; }
        public ObjReflection Reflection { get { return new ObjReflection(this); } } 
    }
}
