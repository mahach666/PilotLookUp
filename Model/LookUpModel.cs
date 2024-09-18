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

        public LookUpModel(List<PilotTypsHelper> dataObjects, IObjectsRepository objectsRepository)
        {
            _dataObjects = dataObjects;
            _objectsRepository = objectsRepository;
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

            var loader = new ObjectLoader(_objectsRepository);

            switch (obj)
            {
                case Guid id:
                    var dataObj = await loader.Load(id);
                    AddToSelection(dataObj);
                    break;

                case IEnumerable<Guid> idEnum:
                    var dataObjes = new List<object>();
                    foreach (var guid in idEnum)
                    {
                        object dataObject = await loader.Load(guid);

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

                // Other cases for collections and dictionaries
                default:
#if DEBUG
                    MessageBox.Show($"Unhandled type: {obj.GetType()}");
#endif
                    break;
            }
        }

        private void AddToSelection(params object[] objects)
        {
            var selection = objects.Select(i => new PilotTypsHelper(i)).ToList();
            if (selection.Any())
            {
                new LookSeleсtion(selection, _objectsRepository);
            }
        }

        private void AddToSelection(IEnumerable<object> objects)
        {
            var selection = objects.Select(i => new PilotTypsHelper(i)).ToList();
            if (selection.Any())
            {
                new LookSeleсtion(selection, _objectsRepository);
            }
        }
    }
}