using PilotLookUp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal abstract class TypeHelper
    {
        public string TypeFullName { get { return LookUpObject.GetType().FullName; } }
        public string Type { get { return LookUpObject.GetType().ToString(); } }
        public string Name { get; set; }
        public object LookUpObject { get; set; }
        public ObjReflection Reflection { get { return new ObjReflection(this); } } }
    }
}
