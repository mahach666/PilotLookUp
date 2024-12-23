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
        public string Name { get; set; }
        public object LookUpObject { get; set; }
        public IObjectsRepository ObjectsRepository { get; }
        public ObjReflection Reflection { get { return LookUpObject == null ? ObjReflection.Empty() : new ObjReflection(this); } }
    }
}
