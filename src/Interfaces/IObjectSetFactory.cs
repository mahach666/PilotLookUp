using System.Reflection;
using PilotLookUp.Objects;

namespace PilotLookUp.Interfaces
{
    public interface IObjectSetFactory
    {
        ObjectSet Create(MemberInfo memberInfo);
    }
} 