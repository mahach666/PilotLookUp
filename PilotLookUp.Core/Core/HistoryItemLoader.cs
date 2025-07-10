using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Data;
using PilotLookUp.Core.Objects;
using System;
using System.Threading.Tasks;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.Core.Core
{
    // TODO: IHistoryItem not found in SDK
    /*
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
            return await _repository.GetHistoryItemWithTimeout(guid);
        }

        public async Task<IHistoryItem> LoadWithTimeout(Guid guid, int timeoutMilliseconds = 300)
        {
            var timeoutTask = Task.Delay(timeoutMilliseconds);
            var loadTask = LoadData(guid);

            var completedTask = await Task.WhenAny(loadTask, timeoutTask);
            if (completedTask == timeoutTask)
            {
                throw new TimeoutException($"Loading history item {guid} timed out after {timeoutMilliseconds}ms");
            }

            return await loadTask;
        }

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(IHistoryItem value)
        {
            if (value.ChangesetId >= _changesetId)
            {
                _tcs.TrySetResult(value);
                _subscription?.Dispose();
            }
        }
    }
    */
}
