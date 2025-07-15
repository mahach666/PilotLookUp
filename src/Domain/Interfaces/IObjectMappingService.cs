using System.Collections.Generic;

namespace PilotLookUp.Domain.Interfaces
{
    public interface IObjectMappingService
    {
        IPilotObjectHelper Wrap(object rawObject);
        IEnumerable<IPilotObjectHelper> WrapMany(IEnumerable<object> rawObjects);
    }
} 