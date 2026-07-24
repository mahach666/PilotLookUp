using System;

namespace PilotLookUp.Objects
{
    public enum BimModelStage
    {
        Opened,
        Loaded,
        Closed,
        Disposed
    }

    // Жизненный цикл модели в Pilot-BIM
    public sealed class BimModelLifecycleRecord
    {
        public BimModelLifecycleRecord(BimModelStage stage, Guid modelId, Guid tabId, DateTime modelVersion)
        {
            Stage = stage;
            ModelId = modelId;
            TabId = tabId;
            ModelVersion = modelVersion;
            DateTime = DateTime.Now;
        }

        public BimModelStage Stage { get; }
        public Guid ModelId { get; }
        public Guid TabId { get; }
        public DateTime ModelVersion { get; }
        public DateTime DateTime { get; }
    }
}
