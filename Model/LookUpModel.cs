using Ascon.Pilot.SDK;
using PilotLookUp.Commands;
using PilotLookUp.Core;
using PilotLookUp.Extensions;
using PilotLookUp.Objects;
using PilotLookUp.Utils;
using PilotLookUp.ViewBuilders;
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
        private List<PilotObjectHelper> _dataObjects { get; }
        private IObjectsRepository _objectsRepository { get; }
        private ObjectLoader _loader { get; }

        public LookUpModel(List<PilotObjectHelper> dataObjects, IObjectsRepository objectsRepository)
        {
            _dataObjects = dataObjects;
            _objectsRepository = objectsRepository;

            _loader = new ObjectLoader(_objectsRepository);
        }

        public List<PilotObjectHelper> SelectionDataObjects => _dataObjects;

        public ObjReflection GetInfo(PilotObjectHelper dataObject)
        {
            return dataObject.Reflection;
            //return new ObjReflection(dataObject);
        }

        public async Task DataGridSelector(object obj)
        {
            if (obj == null) return;
            PilotObjectMap.Updaate(_objectsRepository);
            AddToSelection(obj);
        }

        private async void AddToSelection<T>(IEnumerable<T> objects)
        {
            //var selection = objects.Select(i => PilotObjectMap.Wrap(i)).ToList();

            var selection = new List<PilotObjectHelper>();
            foreach (object obj in objects)
            {
                if (obj is Guid) selection.Add(PilotObjectMap.Wrap(await _objectsRepository.GetObject((Guid)obj), _objectsRepository));
                else
                    selection.Add(PilotObjectMap.Wrap(obj, _objectsRepository));
            }

            if (selection.Any())
            {
                new LookSeleсtion(selection, _objectsRepository);
            }
        }
        private void AddToSelection<TKey, TValue>(IDictionary<TKey, TValue> objects)
        {
            var selection = objects.Select(i => PilotObjectMap.Wrap(i, _objectsRepository)).ToList();
            if (selection.Any())
            {
                new LookSeleсtion(selection, _objectsRepository);
            }
        }
        private async void AddToSelection(object obj)
        {
            if (obj is Guid)
                new LookSeleсtion(new List<PilotObjectHelper> { PilotObjectMap.Wrap(await _objectsRepository.GetObject((Guid)obj)) }, _objectsRepository);
            else
                new LookSeleсtion(new List<PilotObjectHelper> { PilotObjectMap.Wrap(obj) }, _objectsRepository);
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