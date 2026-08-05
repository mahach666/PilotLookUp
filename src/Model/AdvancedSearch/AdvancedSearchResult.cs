using System;
using System.Collections.Generic;

namespace PilotLookUp.Model.AdvancedSearch
{
    public sealed class AdvancedSearchResult
    {
        public AdvancedSearchResult(IReadOnlyList<Guid> ids, long total, bool timedOut)
        {
            Ids = ids ?? Array.Empty<Guid>();
            Total = total;
            TimedOut = timedOut;
        }

        public IReadOnlyList<Guid> Ids { get; }
        public long Total { get; }
        public bool TimedOut { get; }
    }
}
