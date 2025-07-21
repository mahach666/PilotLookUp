using PilotLookUp.Objects;
using System;

namespace PilotLookUp.Interfaces
{
    public interface ISelectedService
    {
        ObjectSet Selected { get; }
        void UpdateSelected(MarshalByRefObject context);
    }
} 