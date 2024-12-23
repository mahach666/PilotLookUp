using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class UserStateHelper : PilotObjectHelper
    {
        public UserStateHelper(IUserState obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            LookUpObject = obj;
            Name = obj.Title;
        }
    }
}
