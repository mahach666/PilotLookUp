using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Model
{
    public class PilotTypsHelper
    {
        public PilotTypsHelper(object pilotObj)
        {
            PilotObj = pilotObj;

            if (PilotObj == null) return;


            else if (PilotObj is IDataObject idataObj)
            {
                DisplayName = idataObj.DisplayName;
            }

            else if (PilotObj is IType pilotType)
            {
                DisplayName = pilotType.Name;
            }

            else if (PilotObj is IAttribute pilotAttr)
            {
                DisplayName = pilotAttr.Title;
            }
        }


        public object PilotObj { get; }
        public  string DisplayName { get; }
    }
}
