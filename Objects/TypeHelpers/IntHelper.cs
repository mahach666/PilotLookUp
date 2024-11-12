using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class IntHelper : PilotObjectHelper
    {
        public IntHelper(int value, PilotObjectHelper sender)
        {
            if (sender.LookUpObject is IType type)
            {
                if (type.Id == value)
                {
                    LookUpObject = type;
                    Name = type.Title;
                }
            }
            else if (sender.LookUpObject is IPerson person)
            {
                if (person.Id == value)
                {
                    LookUpObject = person;
                    Name = person.DisplayName;
                }
            }
            else
            {

            }
        }
    }
}
