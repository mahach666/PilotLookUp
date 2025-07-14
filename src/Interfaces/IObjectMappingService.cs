using PilotLookUp.Objects;
using System.Collections.Generic;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Interfaces
{
    public interface IObjectMappingService
    {
        IPilotObjectHelper Wrap(object rawObject);
        IEnumerable<IPilotObjectHelper> WrapMany(IEnumerable<object> rawObjects);
    }
} 