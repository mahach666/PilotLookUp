using Ascon.Pilot.Bim.SDK;
using Ascon.Pilot.Bim.SDK.ModelTab.Menu;
using Ascon.Pilot.SDK;
using PilotLookUp.Objects;
using System;

namespace PilotLookUp.Interfaces
{
    public interface ISelectedService
    {
        ObjectSet Selected { get; }
        void UpdateSelected(MarshalByRefObject context, string source = null);
        void UpdateSelected(SignatureRequestsContext context, string source = null);
        void UpdateSelected(ModelContext context, string source = null);
    }
} 