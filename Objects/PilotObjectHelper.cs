using Ascon.Pilot.SDK;
using PilotLookUp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Objects
{
    public abstract class PilotObjectHelper
    {
        public PilotObjectHelper(IObjectsRepository objectsRepository)
        {
            ObjectsRepository = objectsRepository;
        }
        protected string _name { get; set; }
        public string Name { get => _name; }
        public object _lookUpObject { get; set; }
        public object LookUpObject { get=> _lookUpObject; }
        public IObjectsRepository ObjectsRepository { get; }
        public ObjReflection Reflection { get { return LookUpObject == null ? ObjReflection.Empty() : new ObjReflection(this); } }
    }
}
