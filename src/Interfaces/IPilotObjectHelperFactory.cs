using PilotLookUp.Objects;

namespace PilotLookUp.Interfaces
{
    public interface IPilotObjectHelperFactory
    {
        PilotObjectHelper Create(string name, string stringId, object lookUpObject, bool isLookable);
    }
} 