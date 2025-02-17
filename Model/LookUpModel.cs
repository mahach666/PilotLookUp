using Ascon.Pilot.SDK;
using PilotLookUp.Core;
using PilotLookUp.Extensions;
using PilotLookUp.Objects;
using PilotLookUp.Objects.TypeHelpers;
using PilotLookUp.Utils;
using PilotLookUp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;



namespace PilotLookUp.Model
{
    internal class LookUpModel
    {
        private ObjectSet _dataObjects { get; }
        private const string _revokedTaskState = "revoked";
        private IObjectsRepository _objectsRepository { get; }
        private ITabServiceProvider _tabServiceProvider { get; }

        public LookUpModel(ObjectSet dataObjects, IObjectsRepository objectsRepository, ITabServiceProvider tabServiceProvider)
        {
            _dataObjects = dataObjects;
            _objectsRepository = objectsRepository;
            _tabServiceProvider = tabServiceProvider;
        }

        public ObjectSet SelectionDataObjects => _dataObjects;


        public void DataGridSelector(ObjectSet obj)
        {
            if (obj == null) return;
            ViewBuilder.LookSeleсtion(obj, _objectsRepository, _tabServiceProvider);
        }

        public async Task<List<ObjectSet>> Info(PilotObjectHelper sender)
        {
            var res = new List<ObjectSet>();
            foreach (var pair in sender.Reflection.KeyValuePairs)
            {
                ObjectSet newPilotObj = await new Tracer(_objectsRepository, sender, pair.Key).Trace(pair.Value);
                res.Add(newPilotObj);
            }
            return res;
        }

        public async Task<ObjectSet> SearchByString(string request)
        {
            var tracer = new Tracer(_objectsRepository, null, null);
            if (Guid.TryParse(request, out var id))
            {
                var res = await tracer.Trace(await _objectsRepository.GetObjByGuid(id));
                return res;
            }
            else if (int.TryParse(request, out var intId))
            {
                var res = new ObjectSet(null);
                var person = _objectsRepository.GetPerson(intId);
                var orgUnit = _objectsRepository.GetOrganisationUnit(intId);
                var iType = _objectsRepository.GetType(intId);
                if (person != null) { res.AddRange(await tracer.Trace(person)); }
                if (orgUnit != null) { res.AddRange(await tracer.Trace(orgUnit)); }
                if (iType != null) { res.AddRange(await tracer.Trace(iType)); }
                var distSet = new ObjectSet(null);
                distSet.AddRange(res.Distinct());              
                return distSet;
            }
            return null;
        }

        public LookUpVM GetDBLookUpVM()
        {
            var pilotObjectMap = new PilotObjectMap(_objectsRepository);
            var repo = new ObjectSet(null) { pilotObjectMap.Wrap(_objectsRepository) };
            return GetCastomLookUpVM(repo.FirstOrDefault());
        }

        public LookUpVM GetCastomLookUpVM(PilotObjectHelper pilotObjectHelper)
        {
            var repo = new ObjectSet(null) { pilotObjectHelper };
            var vm = new LookUpVM(this);
            vm.SelectionDataObjects = repo.Select(x => new ListItemVM(x)).ToList(); ;
            return vm;
        }

        public void GoTo(IDataObject dataObject)
        {
            _tabServiceProvider.ShowElement(dataObject.Id);
        }

        public async Task<DataObjectHelper> FindLastParentHelper(IDataObject dataObject)
        {
            if (dataObject != null)
            {
                var parent = await dataObject.FindLastParrent(_objectsRepository);
                var objSet = await new Tracer(_objectsRepository, null, null).Trace(parent);
                return objSet.FirstOrDefault() is DataObjectHelper result ? result : null;
            }
            return null;
        }

        public async Task<ListItemVM> FillChild(ListItemVM lastParrent)
        {
            await BuildChildNodes(lastParrent);
            return lastParrent;
        }

        public async Task BuildChildNodes(ListItemVM lastParrent)
        {
            var sad = lastParrent.PilotObjectHelper.LookUpObject as IDataObject;
            List<Guid> children = sad.Children.ToList();  // Метод получения детей по ID
            ObjectSet newPilotObj = await new Tracer(_objectsRepository, null, null).Trace(children);
            foreach (var dataObjectHelper in newPilotObj)
            {
                var childNode = new ListItemVM(dataObjectHelper);
                if (lastParrent.Children != null)
                {
                    lastParrent.Children.Add(childNode);
                }
                else
                {
                    lastParrent.Children = new ObservableCollection<ListItemVM>()
                    {
                        childNode
                    };
                }
                await BuildChildNodes(childNode); // Рекурсия для вложенных детей

            }
        }
        internal async Task<List<IDataObject>> GetChildrensWithTimeout(IObjectsRepository objectsRepository, IDataObject currentObject, int timeoutMilliseconds = 300)
        {
            var loader = new ObjectLoader(objectsRepository);
            var childrensId = currentObject.Children;
            List<IDataObject> childrens = new List<IDataObject>();
            foreach (var child in childrensId)
            {
                var childObj = await loader.LoadWithTimeout(child);
                childrens.Add(childObj);
            }
            return childrens;
        }

        /// <summary>
        /// По карточке объекта ищет всязи связи с типо задания. Если задание в процессе то добавляет уникальный процесс, а не задания
        /// </summary>
        /// <param name="objectHelper"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal async Task<List<PilotObjectHelper>> FindAllLastParrent(PilotObjectHelper objectHelper, bool findRevoked = false)
        {
            List<PilotObjectHelper> pilotObjectHelpers = new List<PilotObjectHelper>();
            var loader = new ObjectLoader(_objectsRepository);
            IDataObject dataObject = objectHelper.LookUpObject as IDataObject;
            var listId = dataObject.Relations.Where(it => it.Type == ObjectRelationType.TaskAttachments).Select(fd => fd.TargetId).ToList();
            foreach (var child in listId)
            {
                IDataObject taskObject = await loader.LoadWithTimeout(child);
                bool isTask = taskObject.Type.Name.StartsWith("task_");
                if (isTask)
                {
                    if (!findRevoked) // Пропуск отозванных заданий
                    {
                        var atrState = taskObject.Attributes["state"];
                        if (atrState != null)
                        {
                            Guid guidState = new Guid(atrState.ToString());
                            var stateName = await _objectsRepository.GetObjByGuid(guidState, 0);
                            IUserState userState = stateName as IUserState;
                            if (userState != null)
                            {
                                if (userState.Name == _revokedTaskState)
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    DataObjectHelper lastParrent = await FindLastParentHelper(taskObject);
                    if (!pilotObjectHelpers.Select(it => it.StringId).Contains(lastParrent.StringId))
                    {
                        pilotObjectHelpers.Add(lastParrent);
                    }
                }

            }
            return pilotObjectHelpers;
        }
    }
}