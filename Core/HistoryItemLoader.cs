using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Core
{
    public class HistoryItemLoader : IObserver<IHistoryItem>
    {
        private readonly IObjectsRepository _repository;
        private IDisposable _subscription;
        private TaskCompletionSource<IHistoryItem> _tcs;
        private long _changesetId;

        public HistoryItemLoader(IObjectsRepository repository)
        {
            _repository = repository;
        }

        public Task<IHistoryItem> Load(Guid id, long changesetId = 0)
        {
            _changesetId = changesetId;
            _tcs = new TaskCompletionSource<IHistoryItem>();
            _subscription = _repository.GetHistoryItems(new[] { id }).Subscribe(this);

            return _tcs.Task;
        }

        private async Task<IHistoryItem> LoadData(Guid guid)
        {
            return await Load(guid);
        }

        public async Task<IHistoryItem> LoadWithTimeout(Guid guid, int timeoutMilliseconds = 300)
        {
            var loadTask = LoadData(guid);
            var delayTask = Task.Delay(timeoutMilliseconds);

            // Ожидаем завершения одной из задач — загрузки данных или таймаута
            var completedTask = await Task.WhenAny(loadTask, delayTask);

            if (completedTask == loadTask)
            {
                // Если загрузка завершилась, возвращаем результат
                return await loadTask;
            }
            else
            {
                // Если завершился таймаут, возвращаем null
                return null;
            }
        }

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(IHistoryItem value)
        {
            _tcs.TrySetResult(value);
            _subscription.Dispose();
        }
    }
}
