using Ascon.Pilot.SDK;
using PilotLookUp.Core;
using PilotLookUp.Extensions;
using PilotLookUp.Objects;
using PilotLookUp.Objects.TypeHelpers;
using PilotLookUp.Utils;
using PilotLookUp.View.CastomUIElemens;
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
                var res = new List<PilotObjectHelper>();
                var person = _objectsRepository.GetPerson(intId);
                var orgUnit = _objectsRepository.GetOrganisationUnit(intId);
                var iType = _objectsRepository.GetType(intId);
                if (person != null) { res.AddRange(await tracer.Trace(person)); }
                if (orgUnit != null) { res.AddRange(await tracer.Trace(orgUnit)); }
                if (iType != null) { res.AddRange(await tracer.Trace(iType)); }
                var oSet = new ObjectSet(null);
                oSet.AddRange(res.Distinct());
                return oSet;
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
        /// <summary>
        /// Метод по поиску последнего родителя для элемента
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<DataObjectHelper> FindLastParrent(PilotObjectHelper objHelper)
        {
            // Если объекта нет, выводим ошибку
            if (objHelper == null)
            {
                return null;
            }
            IDataObject dataObject = objHelper.LookUpObject as IDataObject;
            // Ищем самого верхнего родителя


            while (dataObject.ParentId != null && dataObject.ParentId.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                dataObject = await _objectsRepository.GetObjectWithTimeout(dataObject.ParentId);
            }
            DataObjectHelper dataObjectHelper = new DataObjectHelper(dataObject, _objectsRepository);
            return dataObjectHelper;
        }

        public async Task<ListItemVM> FillChild(ListItemVM lastParrent)
        {
            await BuildChildNodes(lastParrent);
            return lastParrent;
        }

        public async Task BuildChildNodes(ListItemVM lastParrent)
        {
            IDataObject sad = lastParrent.PilotObjectHelper.LookUpObject as IDataObject;
            var children = await GetChildrensWithTimeout(_objectsRepository, sad);  // Метод получения детей по ID
            foreach (var child in children)
            {
                DataObjectHelper dataObjectHelper = new DataObjectHelper(sad, _objectsRepository);
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
    }
}