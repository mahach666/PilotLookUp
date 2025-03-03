using Ascon.Pilot.SDK;
using PilotLookUp.Objects;
using PilotLookUp.Objects.TypeHelpers;
using PilotLookUp.Utils;
using PilotLookUp.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotLookUp.Model
{
    internal class LookUpModel
    {
        private IObjectsRepository _objectsRepository { get; }
        private ITabServiceProvider _tabServiceProvider { get; }

        public LookUpModel(IObjectsRepository objectsRepository, ITabServiceProvider tabServiceProvider)
        {
            _objectsRepository = objectsRepository;
            _tabServiceProvider = tabServiceProvider;
        }

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
            => await SearchUtils.GetObjByString(_objectsRepository, request);


        public LookUpVM GetDBLookUpVM()
        {
            var pilotObjectMap = new PilotObjectMap(_objectsRepository);
            var repo = new ObjectSet(null) { pilotObjectMap.Wrap(_objectsRepository) };
            return GetCastomLookUpVM(repo);
        }

        public LookUpVM GetCastomLookUpVM(ObjectSet pilotObjectHelper)
        {
            var vm = new LookUpVM(this);
            vm.SelectionDataObjects = pilotObjectHelper.Select(x => new ListItemVM(x)).ToList(); ;
            return vm;
        }

        public void GoTo(IDataObject dataObject)
           => _tabServiceProvider.ShowElement(dataObject.Id);

        public async Task<ListItemVM> FillChild(ListItemVM lastParrent)
            => await TreeViewUtils.FillChild(_objectsRepository, lastParrent);

        public async Task<DataObjectHelper> SearchLastParent(IDataObject dataObject)
           => await SearchUtils.GetLastParent(_objectsRepository, dataObject);

        public async Task<ObjectSet> SearchBaseParentsOfRelations(PilotObjectHelper objectHelper, bool findRevoked = false)
            => await SearchUtils.GetBaseParentsOfRelations(_objectsRepository, objectHelper, findRevoked);
    }
}