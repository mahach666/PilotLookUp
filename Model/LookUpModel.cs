using Ascon.Pilot.SDK;
using PilotLookUp.Commands;
using PilotLookUp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.Model
{
    internal class LookUpModel
    {
        private List<PilotTypsHelper> _dataObjects { get; }
        private IObjectsRepository _objectsRepository { get; }
        private ObjectLoader _loader { get; }

        public LookUpModel(List<PilotTypsHelper> dataObjects, IObjectsRepository objectsRepository)
        {
            _dataObjects = dataObjects;
            _objectsRepository = objectsRepository;

            _loader = new ObjectLoader(_objectsRepository);

            PilotTypsHelper.Loader = new ObjectLoader(_objectsRepository);
        }

        public List<PilotTypsHelper> SelectionDataObjects => _dataObjects;

        public ObjReflection GetInfo(PilotTypsHelper dataObject)
        {
            return new ObjReflection(dataObject);
        }

        public async Task DataGridSelector(object obj)
        {
            if (obj == null) return;

            switch (obj)
            {
                case DateTime date:
                    return;

                case bool boolValue:
                    return;

                case string stringValue:
                    if (Guid.TryParse(stringValue, out Guid resGuid)) AddToSelection(await GetObjByGuid(resGuid));
                    return;

                case Guid id:
                    AddToSelection(await GetObjByGuid(id));
                    break;

                case IEnumerable<Guid> idEnum:
                    var dataObjes = new List<object>();
                    foreach (var guid in idEnum)
                    {
                        object dataObject = await GetObjByGuid(guid);

                        if (dataObject != null)
                        {
                            dataObjes.Add(dataObject);
                        }
                    }
                    AddToSelection(dataObjes);
                    break;

                case int personId:
                    var person = _objectsRepository.GetPerson(personId);
                    AddToSelection(person);
                    break;

                case IEnumerable<int> peopleIdList:
                    var people = _objectsRepository.GetPeople().Where(i => peopleIdList.Contains(i.Id));
                    AddToSelection(people);
                    break;

                case IFilesSnapshot filesSnapshot:
                    AddToSelection(filesSnapshot);
                    break;

                case IType type:
                    AddToSelection(type);
                    break;

                case IPerson personObj:
                    AddToSelection(personObj);
                    break;

                case IPosition position:
                    var orgUnit = _objectsRepository.GetOrganisationUnit(position.Order);
                    AddToSelection(orgUnit);
                    break;

                case IEnumerable<IPosition> positionList:
                    var orgUnits = _objectsRepository.GetOrganisationUnits()
                                    .Where(i => positionList.Select(j => j.Order).Contains(i.Id));
                    AddToSelection(orgUnits);
                    break;

                case IEnumerable<IRelation> relationList:
                    AddToSelection(relationList);
                    break;

                case IEnumerable<IAttribute> attributeList:
                    AddToSelection(attributeList);
                    break;

                case IEnumerable<IFile> fileList:
                    AddToSelection(fileList);
                    break;

                case IEnumerable<IAccessRecord> accessRecordList:
                    AddToSelection(accessRecordList);
                    break;

                case IEnumerable<IFilesSnapshot> fileSnapshotList:
                    AddToSelection(fileSnapshotList);
                    break;

                case IDictionary<string, object> attrDict:
                    AddToSelection(attrDict);
                    break;

                case IDictionary<Guid, int> childretTypes:
                    AddToSelection(childretTypes);
                    break;

                case IDictionary<int, IAccess> accessDict:
                    AddToSelection(accessDict);
                    break;

                case Enum enumObj:
                    AddToSelection(enumObj);
                    break;

                // Other cases for collections and dictionaries
                default:
#if DEBUG
                    MessageBox.Show($"Unhandled type: {obj.GetType()}");
#endif
                    break;
            }
        }

        private void AddToSelection<T>(IEnumerable<T> objects)
        {
            var selection = objects.Select(i => new PilotTypsHelper(i)).ToList();
            if (selection.Any())
            {
                new LookSeleсtion(selection, _objectsRepository);
            }
        }
        private void AddToSelection<TKey, TValue>(IDictionary<TKey, TValue> objects)
        {
            var selection = objects.Select(i => new PilotTypsHelper(i)).ToList();
            if (selection.Any())
            {
                new LookSeleсtion(selection, _objectsRepository);
            }
        }
        private void AddToSelection(object obj)
        {
            new LookSeleсtion(new List<PilotTypsHelper> { new PilotTypsHelper(obj) }, _objectsRepository);
        }

        private async Task<object> GetObjByGuid(Guid guid)
        {
            if (guid == null || guid == Guid.Empty) return null;

            IEnumerable<IUserState> states = _objectsRepository.GetUserStates();
            var state = states.FirstOrDefault(i => i.Id == guid);

            if (state == null)
            {
                var statesMachine = _objectsRepository.GetUserStateMachines();
                var stateMachine = statesMachine.FirstOrDefault(i => i.Id == guid);

                if (stateMachine == null)
                {
                    var dataObj = await _loader.Load(guid);
                    return dataObj;
                }
                return stateMachine;
            }
            return state;
        }
    }
}