using Ascon.Pilot.SDK;
using PilotLookUp.Extensions;
using PilotLookUp.Objects;
using PilotLookUp.ViewBuilders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Model.Utils
{
    public class Tracer
    {
        public Tracer(IObjectsRepository objectsRepository, PilotObjectHelper senderObj, MemberInfo senderMember)
        {
            _pilotObjectMap = new PilotObjectMap(objectsRepository, senderObj);
            _objectsRepository = objectsRepository;
            _objectSet = new ObjectSet(senderMember);
        }

        private PilotObjectMap _pilotObjectMap { get; }
        private IObjectsRepository _objectsRepository { get; }
        private ObjectSet _objectSet { get; set; }

        public async Task<ObjectSet> Trace(object obj)
        {
            if (obj == null) return _objectSet;

            // Определение типа объекта и вызов соответствующей перегрузки
            if (obj is IEnumerable enumerable && !(obj is string))
            {
                return await AddToSelectionEnum(enumerable.Cast<object>());
            }
            else if (obj is IDictionary dictionary)
            {
                List<object> keys = new List<object>(dictionary.Keys.Cast<object>());
                List<object> values = new List<object>(dictionary.Values.Cast<object>());

                return await AddToSelectionDict(keys.Zip(values, (key, value) => new { key, value })
                                         .ToDictionary(x => x.key, x => x.value));
            }
            else
            {
                return await AddToSelection(obj);
            }
        }

        private async Task<ObjectSet> AddToSelectionEnum<T>(IEnumerable<T> objects)
        {
            foreach (object obj in objects)
            {
                if (obj is Guid) _objectSet.Add(_pilotObjectMap.Wrap(await _objectsRepository.GetObject((Guid)obj)));
                else
                    _objectSet.Add(_pilotObjectMap.Wrap(obj));
            }
            return _objectSet;
        }

        private async Task<ObjectSet> AddToSelectionDict<TKey, TValue>(IDictionary<TKey, TValue> objects)
        {
            if (objects is Dictionary<Guid, int> childTypeDict)
            {
                foreach (var item in childTypeDict)
                {
                    var lodetDict = new Dictionary<IDataObject, int>() { [await _objectsRepository.GetObject(item.Key)] = item.Value };
                    _objectSet.Add(_pilotObjectMap.Wrap(lodetDict));
                }
            }

            _objectSet.AddRange(objects.Select(i => _pilotObjectMap.Wrap(i)));
            return _objectSet;
        }

        private async Task<ObjectSet> AddToSelection(object obj)
        {
            if (obj is Guid)
            {
                _objectSet.Add(_pilotObjectMap.Wrap(await _objectsRepository.GetObject((Guid)obj)));
                return _objectSet;
            }
            else
            {
                _objectSet.Add(_pilotObjectMap.Wrap(obj));
                return _objectSet;
            }
        }
    }
}
