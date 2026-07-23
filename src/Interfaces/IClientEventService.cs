using System;

namespace PilotLookUp.Interfaces
{
    public interface IClientEventService
    {
        event Action<object> EventRaised;
        void Raise(object record);
    }
}
