using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System;
using System.Collections.Generic;

namespace PilotLookUp.Model.Services
{
    public class ClientEventService : IClientEventService
    {
        public event Action<ClientEventRecord> EventRaised;

        public void Raise(string name, string message, IReadOnlyList<Guid> objectIds = null, string typeInfo = null)
        {
            EventRaised?.Invoke(new ClientEventRecord(name, message, objectIds, typeInfo));
        }
    }
}
