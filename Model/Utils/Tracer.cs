using Ascon.Pilot.SDK;
using PilotLookUp.Extensions;
using PilotLookUp.Objects;
using PilotLookUp.ViewBuilders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Model.Utils
{
    public class Tracer
    {
        public Tracer(IObjectsRepository objectsRepository, PilotObjectHelper senderObj)
        {
            _pilotObjectMap = new PilotObjectMap(objectsRepository, senderObj);
            _objectsRepository = objectsRepository;
        }

       private PilotObjectMap _pilotObjectMap { get; }
        private IObjectsRepository _objectsRepository { get; }

        public async Task<ObjectSet> Trace(object obj)
        {
            if (obj == null) return new ObjectSet();

            // Определение типа объекта и вызов соответствующей перегрузки
            if (obj is IEnumerable enumerable && !(obj is string))
            {
                return await AddToSelectionEnum(enumerable.Cast<object>());
            }
            else if (obj is IDictionary dictionary)
            {
                List<object> keys = new List<object>(dictionary.Keys.Cast<object>());
                List<object> values = new List<object>(dictionary.Values.Cast<object>());

                return AddToSelectionDict(keys.Zip(values, (key, value) => new { key, value })
                                         .ToDictionary(x => x.key, x => x.value));
            }
            else
            {
                return await AddToSelection(obj);
            }
        }

        private async Task<ObjectSet> AddToSelectionEnum<T>(IEnumerable<T> objects)
        {
            var selection = new ObjectSet();
            foreach (object obj in objects)
            {
                if (obj is Guid) selection.Add(_pilotObjectMap.Wrap(await _objectsRepository.GetObject((Guid)obj)));
                else
                    selection.Add(_pilotObjectMap.Wrap(obj));
            }
            return selection;
        }
        private ObjectSet AddToSelectionDict<TKey, TValue>(IDictionary<TKey, TValue> objects)
        {
            var selection = new ObjectSet(objects.Select(i => _pilotObjectMap.Wrap(i)).ToList());

            return selection;
        }
        private async Task<ObjectSet> AddToSelection(object obj)
        {
            if (obj is Guid)
                return new ObjectSet { _pilotObjectMap.Wrap(await _objectsRepository.GetObject((Guid)obj)) };
            else
                return new ObjectSet { _pilotObjectMap.Wrap(obj) };
        }
    }
}
