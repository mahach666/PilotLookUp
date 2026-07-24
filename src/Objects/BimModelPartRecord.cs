using System;

namespace PilotLookUp.Objects
{
    public enum BimPartKind
    {
        Loading,
        Unloading
    }

    // Запрос загрузки/выгрузки части модели
    public sealed class BimModelPartRecord
    {
        public BimModelPartRecord(Guid modelId, Guid tabId, Guid partId, BimPartKind kind)
        {
            ModelId = modelId;
            TabId = tabId;
            PartId = partId;
            Kind = kind;
            DateTime = DateTime.Now;
        }

        public Guid ModelId { get; }
        public Guid TabId { get; }
        public Guid PartId { get; }
        public BimPartKind Kind { get; }
        public DateTime DateTime { get; }
    }
}
