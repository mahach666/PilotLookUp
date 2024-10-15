using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Core
{
    public class Loader : IObserver<IDataObject>
    {
        private IObjectsRepository _rep;
        private readonly HashSet<Guid> _toLoad = new HashSet<Guid>();
        private readonly Dictionary<Guid, IDataObject> _loaded = new Dictionary<Guid, IDataObject>();
        private IDisposable _subscription;
        private TaskCompletionSource<List<IDataObject>> _tcs;

        public Loader(IObjectsRepository rep)
        {
            _rep = rep;
        }

        public Task<List<IDataObject>> Load(IEnumerable<Guid> Ids)
        {
            _loaded.Clear();
            if (Ids.Count() == 0)
            {
                return Task.FromResult(new List<IDataObject>());
            }
            foreach (var child in Ids)
            {
                _toLoad.Add(child);
            }
            _tcs = new TaskCompletionSource<List<IDataObject>>();
            _subscription = _rep.SubscribeObjects(Ids).Subscribe(this);
            return _tcs.Task;
        }

        public async Task<IDataObject> Load(Guid Id)
        {
            var objects = await Load(new Guid[] { Id });
            return objects.FirstOrDefault();
        }

        public void OnNext(IDataObject value)
        {
            if (value.State != DataState.Loaded)
            {
                return;
            }

            if (_loaded.ContainsKey(value.Id))
            {
                return;
            }
            _loaded.Add(value.Id, value);
            _toLoad.Remove(value.Id);
            if (!_toLoad.Any())
            {
                _tcs.TrySetResult(_loaded.Values.ToList());
                _subscription.Dispose();
            }
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
    }
}
