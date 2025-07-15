using System.Reflection;
using PilotLookUp.Domain.Entities;

namespace PilotLookUp.Domain.Interfaces
{
    public interface IObjectSetFactory
    {
        ObjectSet Create(MemberInfo memberInfo);
    }
} 