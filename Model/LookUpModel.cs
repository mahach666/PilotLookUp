using Ascon.Pilot.SDK;
using PilotLookUp.Extensions;
using PilotLookUp.Objects;
using PilotLookUp.Utils;
using PilotLookUp.ViewBuilders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using PilotLookUp.ViewModel;


namespace PilotLookUp.Model
{
    internal class LookUpModel
    {
        private ObjectSet _dataObjects { get; }
        private IObjectsRepository _objectsRepository { get; }

        public LookUpModel(ObjectSet dataObjects, IObjectsRepository objectsRepository)
        {
            _dataObjects = dataObjects;
            _objectsRepository = objectsRepository;
        }

        public ObjectSet SelectionDataObjects => _dataObjects;


        public void DataGridSelector(ObjectSet obj)
        {
            if (obj == null) return;
            new LookSeleсtion(obj, _objectsRepository);
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
            var vm = new LookUpVM(this);
            vm.SelectionDataObjects = repo;
            return vm;
        }

        public LookUpVM GetCastomLookUpVM(PilotObjectHelper pilotObjectHelper)
        {
            var repo = new ObjectSet(null) { pilotObjectHelper };
            var vm = new LookUpVM(this);
            vm.SelectionDataObjects = repo;
            return vm;
        }
    }
}