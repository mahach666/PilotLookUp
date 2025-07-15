using Ascon.Pilot.SDK;
using System;
using System.Threading.Tasks;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.Infrastructure
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

        private async Task<IDataObject> LoadData(Guid guid)
        {
            return await Load(guid);
        }

        public async Task<IDataObject> LoadWithTimeout(Guid guid, int timeoutMilliseconds = 300)
        {
            var loadTask = LoadData(guid);
            var delayTask = Task.Delay(timeoutMilliseconds);

            var completedTask = await Task.WhenAny(loadTask, delayTask);

            if (completedTask == loadTask)
            {
                return await loadTask;
            }
            else
            {
                return null;
            }
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
