using Ascon.Pilot.SDK;
using PilotLookUp.Model.AdvancedSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotLookUp.Core
{
    // Наблюдатель результатов поиска. Search() эмитит Local, затем Remote и,
    // как показали тесты, OnCompleted может не приходить вовсе - поэтому
    // завершаемся по Remote-эмиссии (финальный ответ сервера), таймаут - запасной
    // выход с локальным результатом. Данные копируются сразу в OnNext.
    // MaxResults сервер применяет не ко всем эмиссиям - лимит режем сами
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

            if (value.Kind == SearchResultKind.Remote)
                _tcs.TrySetResult(true);
        }

        public void OnCompleted() => _tcs.TrySetResult(true);

        public void OnError(Exception error) => _tcs.TrySetException(error);

        public async Task<AdvancedSearchResult> WaitAsync(IObservable<ISearchResult> source,
            int timeoutMilliseconds, int maxResults)
        {
            var subscription = source.Subscribe(this);
            try
            {
                var completed = await Task.WhenAny(_tcs.Task, Task.Delay(timeoutMilliseconds));

                var timedOut = completed != _tcs.Task;
                if (!timedOut)
                    await _tcs.Task; // проброс исключения из OnError

                var ids = _lastIds.Count > maxResults
                    ? _lastIds.Take(maxResults).ToList()
                    : _lastIds;

                return new AdvancedSearchResult(ids, _lastTotal, timedOut);
            }
            finally
            {
                subscription.Dispose();
            }
        }
    }
}
