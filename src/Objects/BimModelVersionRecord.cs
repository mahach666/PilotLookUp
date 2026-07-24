using System;

namespace PilotLookUp.Objects
{
    public enum BimVersionKind
    {
        Added,
        Changed
    }

    // Появление/смена версии модели
    public sealed class BimModelVersionRecord
    {
        public BimModelVersionRecord(Guid modelId, Guid tabId, DateTime version, BimVersionKind kind)
        {
            ModelId = modelId;
            TabId = tabId;
            Version = version;
            Kind = kind;
            DateTime = DateTime.Now;
        }

        public Guid ModelId { get; }
        public Guid TabId { get; }
        public DateTime Version { get; }
        public BimVersionKind Kind { get; }
        public DateTime DateTime { get; }
    }
}
