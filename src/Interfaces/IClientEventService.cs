using PilotLookUp.Objects;
using System;
using System.Collections.Generic;

namespace PilotLookUp.Interfaces
{
    public interface IClientEventService
    {
        event Action<ClientEventRecord> EventRaised;
        void Raise(string name, string message, IReadOnlyList<Guid> objectIds = null, string typeInfo = null);
    }
}
