using Ascon.Pilot.SDK;
using PilotLookUp.Core;
using System;
using System.Threading.Tasks;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.Core
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

        public async Task<IDataObject> LoadWithTimeout(Guid guid, int timeoutMilliseconds = PilotTimeouts.ObjectLoadMs)
        {
            var loadTask = Load(guid);
            var delayTask = Task.Delay(timeoutMilliseconds);

            // Ожидаем завершения одной из задач - загрузки данных или таймаута
            var completedTask = await Task.WhenAny(loadTask, delayTask);

            if (completedTask == loadTask)
            {
                // Если загрузка завершилась, возвращаем результат
                return await loadTask;
            }

            // Таймаут: снимаем подписку, иначе она живет до конца сессии
            _subscription?.Dispose();
            _tcs.TrySetResult(null);
            return null;
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

        public void OnError(Exception error) 
        {
            _tcs?.TrySetException(error);
            _subscription?.Dispose();
        }

        public void OnCompleted() { }
    }
}
