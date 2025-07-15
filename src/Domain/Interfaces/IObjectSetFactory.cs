using PilotLookUp.Domain.Entities;
using System.Reflection;

namespace PilotLookUp.Domain.Interfaces
{
    public interface IObjectSetFactory
    {
        ObjectSet Create(MemberInfo memberInfo);
    }
} 