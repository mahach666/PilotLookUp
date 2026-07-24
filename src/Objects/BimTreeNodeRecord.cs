using Ascon.Pilot.Bim.SDK;
using System;

namespace PilotLookUp.Objects
{
    public enum BimTreeNodeKind
    {
        Expanded,
        Collapsed
    }

    // Разворачивание/сворачивание узла дерева модели
    public sealed class BimTreeNodeRecord
    {
        public BimTreeNodeRecord(Guid modelId, Guid tabId, IModelElementId id, BimTreeNodeKind kind)
        {
            ModelId = modelId;
            TabId = tabId;
            Id = id;
            Kind = kind;
            DateTime = DateTime.Now;
        }

        public Guid ModelId { get; }
        public Guid TabId { get; }
        public IModelElementId Id { get; }
        public BimTreeNodeKind Kind { get; }
        public DateTime DateTime { get; }
    }
}
