using Ascon.Pilot.SDK;
using PilotLookUp.Core;
using PilotLookUp.Extensions;
using PilotLookUp.Model.Utils;
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

        public async Task<ObjectSet> SearchByString(string request) => await SearchUtils.ByString(_objectsRepository, request);


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

        public async Task<ListItemVM> FillChild(ListItemVM lastParrent) => await TreeViewUtils.FillChild(_objectsRepository,lastParrent);
        //{
        //    await BuildChildNodes(lastParrent);
        //    return lastParrent;
        //}

        //public async Task BuildChildNodes(ListItemVM lastParrent)
        //{
        //    var sad = lastParrent.PilotObjectHelper.LookUpObject as IDataObject;
        //    List<Guid> children = sad.Children.ToList();  // Метод получения детей по ID
        //    ObjectSet newPilotObj = await new Tracer(_objectsRepository, null, null).Trace(children);
        //    foreach (var dataObjectHelper in newPilotObj)
        //    {
        //        var childNode = new ListItemVM(dataObjectHelper);
        //        if (lastParrent.Children != null)
        //        {
        //            lastParrent.Children.Add(childNode);
        //        }
        //        else
        //        {
        //            lastParrent.Children = new ObservableCollection<ListItemVM>()
        //            {
        //                childNode
        //            };
        //        }
        //        await BuildChildNodes(childNode); // Рекурсия для вложенных детей
        //    }
        //}
        //internal async Task<List<IDataObject>> GetChildrensWithTimeout(IObjectsRepository objectsRepository, IDataObject currentObject, int timeoutMilliseconds = 300)
        //{
        //    var loader = new ObjectLoader(objectsRepository);
        //    var childrensId = currentObject.Children;
        //    List<IDataObject> childrens = new List<IDataObject>();
        //    foreach (var child in childrensId)
        //    {
        //        var childObj = await loader.LoadWithTimeout(child);
        //        childrens.Add(childObj);
        //    }
        //    return childrens;
        //}

        /// <summary>
        /// По карточке объекта ищет всязи связи с типо задания. Если задание в процессе то добавляет уникальный процесс, а не задания
        /// </summary>
        /// <param name="objectHelper"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal async Task<ObjectSet> FindAllLastParrent(PilotObjectHelper objectHelper, bool findRevoked = false)
        {
            ObjectSet pilotObjectHelpers = new ObjectSet(null);
            ObjectSet childrenSet;

            if (objectHelper.LookUpObject is IDataObject dataObject)
            {
                childrenSet = await new Tracer(_objectsRepository, null, null).Trace(dataObject.Children);
            }
            else return null;

            foreach (var child in childrenSet)
            {
                if (child is DataObjectHelper dataHelp)
                {
                    if (dataHelp.IsTask)
                    {
                        if (!findRevoked) // Пропуск отозванных заданий
                        {
                            if (dataHelp.IsRevokedTask)
                            {
                                continue;
                            }
                        }
                        DataObjectHelper lastParrent = await FindLastParentHelper(dataObject);
                        if (!pilotObjectHelpers.Select(it => it.StringId).Contains(lastParrent.StringId))
                        {
                            pilotObjectHelpers.Add(lastParrent);
                        }
                    }
                }
            }
            return pilotObjectHelpers;
        }
    }
}