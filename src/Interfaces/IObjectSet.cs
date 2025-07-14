using System.Collections.Generic;
using System.Reflection;
using PilotLookUp.Objects;

namespace PilotLookUp.Interfaces
{
    public interface IObjectSet : IEnumerable<PilotObjectHelper>
    {
        int Count { get; }
        PilotObjectHelper this[int index] { get; set; }
        void Add(PilotObjectHelper item);
        void AddRange(IEnumerable<PilotObjectHelper> items);
        bool Remove(PilotObjectHelper item);
        void Clear();
        bool Contains(PilotObjectHelper item);
        MemberInfo MemberInfo { get; }
        // Можно добавить другие методы по необходимости
    }
} 