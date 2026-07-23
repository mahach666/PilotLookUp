using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Data;
using System;
using System.Threading.Tasks;

namespace PilotLookUp.Core
{
    public class HistoryItemLoader : IObserver<IHistoryItem>
    {
        private readonly IObjectsRepository _repository;
        private IDisposable _subscription;
        private TaskCompletionSource<IHistoryItem> _tcs;

        public HistoryItemLoader(IObjectsRepository repository)
        {
            _repository = repository;
        }

        public Task<IHistoryItem> Load(Guid id)
        {
            _tcs = new TaskCompletionSource<IHistoryItem>();
            _subscription = _repository.GetHistoryItems(new[] { id }).Subscribe(this);

            return _tcs.Task;
        }

        public async Task<IHistoryItem> LoadWithTimeout(Guid guid, int timeoutMilliseconds = PilotTimeouts.ObjectLoadMs)
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

        public void OnCompleted() { }

        public void OnError(Exception error)
        {
            _tcs?.TrySetException(error);
            _subscription?.Dispose();
        }

        public void OnNext(IHistoryItem value)
        {
            _tcs.TrySetResult(value);
            _subscription.Dispose();
        }
    }
}
