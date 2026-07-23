using PilotLookUp.Interfaces;
using System;

namespace PilotLookUp.Model.Services
{
    public class ClientEventService : IClientEventService
    {
        public event Action<object> EventRaised;

        public void Raise(object record)
        {
            EventRaised?.Invoke(record);
        }
    }
}
