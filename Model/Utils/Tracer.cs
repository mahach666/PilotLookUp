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
        private IObjectsRepository _objectsRepository { get; set; }
        private PilotObjectHelper _sender { get; set; }

        public async Task<ObjectSet> Trace(IObjectsRepository objectsRepository, PilotObjectHelper sender, object obj)
        {
            if (obj == null) return new ObjectSet();

            _objectsRepository = objectsRepository;
            _sender = sender;
            PilotObjectMap.Updaate(_objectsRepository);

            // Определение типа объекта и вызов соответствующей перегрузки
            if (obj is IEnumerable enumerable)
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
            //var selection = objects.Select(i => PilotObjectMap.Wrap(i)).ToList();

            var selection = new ObjectSet();
            foreach (object obj in objects)
            {
                if (obj is Guid) selection.Add(PilotObjectMap.Wrap(await _objectsRepository.GetObject((Guid)obj), _sender));
                else
                    selection.Add(PilotObjectMap.Wrap(obj, _sender));
            }

            //if (selection.Any())
            //{
            //    new LookSeleсtion(selection, _objectsRepository);
            //}
            return selection;
        }
        private ObjectSet AddToSelectionDict<TKey, TValue>(IDictionary<TKey, TValue> objects)
        {
            var selection = new ObjectSet(objects.Select(i => PilotObjectMap.Wrap(i, _sender)).ToList());
            //if (selection.Any())
            //{
            return selection;
            //}   

        }
        private async Task<ObjectSet> AddToSelection(object obj)
        {
            if (obj is Guid)
                return new ObjectSet { PilotObjectMap.Wrap(await _objectsRepository.GetObject((Guid)obj), _sender) };
            else
                return new ObjectSet { PilotObjectMap.Wrap(obj, _sender) };
        }
    }
}
