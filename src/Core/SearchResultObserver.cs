using Ascon.Pilot.SDK;
using PilotLookUp.Model.AdvancedSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotLookUp.Core
{
    // Наблюдатель результатов поиска (по образцу ObjectLoader).
    // Search() эмитит несколько результатов (Local, затем Remote) - берем последний.
    // Данные копируются сразу в OnNext: жизненный цикл ISearchResult не гарантирован
    public class SearchResultObserver : IObserver<ISearchResult>
    {
        private readonly TaskCompletionSource<bool> _tcs =
            new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        private List<Guid> _lastIds = new List<Guid>();
        private long _lastTotal;

        public void OnNext(ISearchResult value)
        {
            _lastIds = value.Result?.ToList() ?? new List<Guid>();
            _lastTotal = value.Total;
        }

        public void OnCompleted() => _tcs.TrySetResult(true);

        public void OnError(Exception error) => _tcs.TrySetException(error);

        public async Task<AdvancedSearchResult> WaitAsync(IObservable<ISearchResult> source, int timeoutMilliseconds)
        {
            var subscription = source.Subscribe(this);
            try
            {
                var completed = await Task.WhenAny(_tcs.Task, Task.Delay(timeoutMilliseconds));

                if (completed == _tcs.Task)
                {
                    await _tcs.Task; // проброс исключения из OnError
                    return new AdvancedSearchResult(_lastIds, _lastTotal, false);
                }

                // Таймаут: возвращаем то, что успело прийти (обычно локальный результат)
                return new AdvancedSearchResult(_lastIds, _lastTotal, true);
            }
            finally
            {
                subscription.Dispose();
            }
        }
    }
}
