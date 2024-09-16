using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Utils
{
    public class ObjectLoader : IObserver<IDataObject>
    {
        private readonly IObjectsRepository _repository;
        private IDisposable _subscription;
        private TaskCompletionSource<IDataObject> _tcs;
        private long _changesetId;

        public ObjectLoader(IObjectsRepository repository)
        {
            _repository = repository;
        }

        public Task<IDataObject> Load(Guid id, long changesetId = 0)
        {
            _changesetId = changesetId;
            _tcs = new TaskCompletionSource<IDataObject>();
            _subscription = _repository.SubscribeObjects(new[] { id }).Subscribe(this);
            return _tcs.Task;
        }

        public void OnNext(IDataObject value)
        {
            if (value.State != DataState.Loaded)
                return;

            if (value.LastChange() < _changesetId)
                return;

            _tcs.TrySetResult(value);
            _subscription.Dispose();
        }

        public void OnError(Exception error) { }

        public void OnCompleted() { }
    }
}
