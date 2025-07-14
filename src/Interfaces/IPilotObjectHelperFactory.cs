using PilotLookUp.Objects;

namespace PilotLookUp.Interfaces
{
    public interface IPilotObjectHelperFactory
    {
        IPilotObjectHelper Create(string name, string stringId, object lookUpObject, bool isLookable);
    }
} 