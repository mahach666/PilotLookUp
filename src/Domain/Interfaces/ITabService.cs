using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Domain.Interfaces
{
    public interface ITabService
    {
        public void GoTo(IDataObject dataObject);
    }
}
