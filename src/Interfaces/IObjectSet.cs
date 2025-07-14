using System.Collections.Generic;
using System.Reflection;
using PilotLookUp.Objects;

namespace PilotLookUp.Interfaces
{
    public interface IObjectSet : IEnumerable<IPilotObjectHelper>
    {
        int Count { get; }
        IPilotObjectHelper this[int index] { get; set; }
        void Add(IPilotObjectHelper item);
        void AddRange(IEnumerable<IPilotObjectHelper> items);
        bool Remove(IPilotObjectHelper item);
        void Clear();
        bool Contains(IPilotObjectHelper item);
        MemberInfo MemberInfo { get; }
        // Можно добавить другие методы по необходимости
    }
} 