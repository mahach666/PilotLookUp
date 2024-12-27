using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class GuidHelper : PilotObjectHelper
    {
        public GuidHelper(Guid value, IObjectsRepository objectsRepository, PilotObjectHelper sender, MemberInfo senderMember) : base(objectsRepository)
        {
            if (senderMember != null)
            {
                if (senderMember.Name == "Id")
                {
                    _lookUpObject = sender.LookUpObject;
                    _name = sender.Name;
                    _isLookable = true;
                    _stringId = sender.StringId;
                }
                else
                {
                    _lookUpObject = value;
                    _name = value.ToString();
                    _isLookable = false;
                }
            }
            else
            {
                _lookUpObject = value;
                _name = value.ToString();
                _isLookable = false;
            }
        }
    }
}
