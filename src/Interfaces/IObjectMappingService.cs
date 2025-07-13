using PilotLookUp.Objects;
using System.Collections.Generic;

namespace PilotLookUp.Interfaces
{
    public interface IObjectMappingService
    {
        PilotObjectHelper Wrap(object rawObject);
        IEnumerable<PilotObjectHelper> WrapMany(IEnumerable<object> rawObjects);
    }
} 