using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Model
{
    internal class PilotTypsHelper<T>
    {
        internal PilotTypsHelper(T pilotObj)
        {
            PilotObj = pilotObj;
            ObjType = PilotObj.GetType();
        }
        public T PilotObj { get; }
        public Type ObjType { get; }
    }
}
