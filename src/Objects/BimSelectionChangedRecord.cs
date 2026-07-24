using Ascon.Pilot.Bim.SDK;
using System;
using System.Collections.Generic;

namespace PilotLookUp.Objects
{
    // Изменение выделения в сцене Pilot-BIM
    public sealed class BimSelectionChangedRecord
    {
        public BimSelectionChangedRecord(Guid modelId, Guid tabId, IReadOnlyList<IModelElementId> selectedIds)
        {
            ModelId = modelId;
            TabId = tabId;
            SelectedIds = selectedIds ?? Array.Empty<IModelElementId>();
            SelectedCount = SelectedIds.Count;
            DateTime = DateTime.Now;
        }

        public Guid ModelId { get; }
        public Guid TabId { get; }
        public IReadOnlyList<IModelElementId> SelectedIds { get; }
        public int SelectedCount { get; }
        public DateTime DateTime { get; }
    }
}
