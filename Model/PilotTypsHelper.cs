using Ascon.Pilot.SDK;
using PilotLookUp.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.Model
{
    public class PilotTypsHelper
    {
        public PilotTypsHelper(object pilotObj, IObjectsRepository objectsRepository = null)
        {
            _objectsRepository = objectsRepository;

            PilotObj = pilotObj;
            if (PilotObj == null) return;

            // Используем старый switch с проверками типов
            switch (PilotObj)
            {
                case IDataObject idataObj:
                    DisplayName = idataObj.DisplayName;
                    break;
                case IType pilotType:
                    DisplayName = pilotType.Title;
                    break;
                case IPerson person:
                    DisplayName = person.DisplayName;
                    break;
                case IUserState userStateMachine:
                    DisplayName = userStateMachine.Title;
                    break;
                case IUserStateMachine userState:
                    DisplayName = userState.Title;
                    break;
                case IAttribute pilotAttr:
                    DisplayName = pilotAttr.Title;
                    break;
                case KeyValuePair<string, object> keyValuePair:
                    DisplayName = keyValuePair.Key;
                    break;
                case KeyValuePair<Guid, int> childrenTypes:
                    DisplayName = childrenTypes.Key.ToString();
                    break;
                case KeyValuePair<int, IAccess> accessDict:
                    DisplayName = accessDict.Value.ToString();
                    break;
                case KeyValuePair<Guid, IEnumerable<ITransition>> transitionsDict:
                    DisplayName = _objectsRepository?.GetUserStates().FirstOrDefault(i => i.Id == transitionsDict.Key)?.Title ?? "invalid";
                    break;
                case IRelation relation:
                    DisplayName = relation.Name;
                    break;
                case IFile file:
                    DisplayName = file.Name;
                    break;
                case IAccess access:
                    DisplayName = access.ToString();
                    break;
                case IAccessRecord accessRecord:
                    DisplayName = accessRecord.ToString();
                    break;
                case IFilesSnapshot filesSnapshot:
                    DisplayName = filesSnapshot.Created.ToString();
                    break;
                case IPosition position:
                    DisplayName = position.Order.ToString();
                    break;
                case IOrganisationUnit organisationUnit:
                    DisplayName = organisationUnit.Title;
                    break;
                case ITransition transition:
                    DisplayName = transition.DisplayName;
                    break;
                case Enum dataEnum:
                    DisplayName = dataEnum.ToString();
                    break;
                default:
                    MessageBox.Show("Invalid type", nameof(pilotObj));
                    break;
            }
        }

        public object PilotObj { get; }
        public string DisplayName { get; }
        //public static ObjectLoader Loader { private get; set; }
        private IObjectsRepository _objectsRepository { get; }

    }


}
