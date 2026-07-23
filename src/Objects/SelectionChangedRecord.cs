using System;
using System.Collections.Generic;

namespace PilotLookUp.Objects
{
    // Событие изменения выделения в клиенте.
    // Свойства отображаются в мониторе рефлексией как есть
    public sealed class SelectionChangedRecord
    {
        public SelectionChangedRecord(
            string source,
            string contextName,
            bool? isContext,
            IReadOnlyList<Guid> selectedObjectIds,
            int shortcutsCount,
            int selectedCount)
        {
            Source = source;
            ContextName = contextName;
            IsContext = isContext;
            SelectedObjectIds = selectedObjectIds ?? Array.Empty<Guid>();
            ShortcutsCount = shortcutsCount;
            SelectedCount = selectedCount;
            DateTime = DateTime.Now;
        }

        public string Source { get; }
        public string ContextName { get; }
        public bool? IsContext { get; }
        public IReadOnlyList<Guid> SelectedObjectIds { get; }
        public int ShortcutsCount { get; }
        public int SelectedCount { get; }
        public DateTime DateTime { get; }
    }
}
