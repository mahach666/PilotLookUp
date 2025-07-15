using PilotLookUp.Objects;
using System.Collections.Generic;
using PilotLookUp.Domain.Interfaces;

namespace PilotLookUp.Domain.Interfaces
{
    public interface IObjectMappingService
    {
        IPilotObjectHelper Wrap(object rawObject);
        IEnumerable<IPilotObjectHelper> WrapMany(IEnumerable<object> rawObjects);
    }
} 