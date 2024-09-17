using Ascon.Pilot.SDK;
using PilotLookUp.Utils;
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
                DisplayName = pilotType.Title;
            }

            else if (PilotObj is IPerson person)
            {
                DisplayName = person.DisplayName;
            }

            else if (PilotObj is IAttribute pilotAttr)
            {
                DisplayName = pilotAttr.Title;
            }

            else if (PilotObj is KeyValuePair<string, object> keyValuePair)
            {
                DisplayName = keyValuePair.Key;
            }

            else if (PilotObj is KeyValuePair<Guid, int> childrenTypes)
            {
                DisplayName = childrenTypes.Key.ToString();
            }

            else if (PilotObj is KeyValuePair<int, IAccess> accessDict)
            {
                DisplayName = accessDict.Value.ToString();
            }

            else if (PilotObj is IRelation relation)
            {
                DisplayName = relation.Name;
            }

            else if (PilotObj is IFile file)
            {
                DisplayName = file.Name;
            }

            //else if (PilotObj is IAccess access)
            //{
            //    DisplayName = access.ToString();
            //}

            else if (PilotObj is IAccessRecord accessRecord)
            {
                DisplayName = accessRecord.ToString();
            }

            else if (PilotObj is IFilesSnapshot filesSnapshot)
            {
                DisplayName = filesSnapshot.Created.ToString();
            }

            else if (PilotObj.GetType().IsEnum)
            {
                var dataEnum = PilotObj as Enum;
                DisplayName = dataEnum.ToString();
            }


        }

        public object PilotObj { get; }
        public string DisplayName { get; }
        public static ObjectLoader Loader { private get; set; }
    }
}
