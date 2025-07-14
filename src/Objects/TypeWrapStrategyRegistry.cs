using System;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Objects
{
    public class TypeWrapStrategyRegistry
    {
        private readonly List<ITypeWrapStrategy> _strategies = new List<ITypeWrapStrategy>();

        public void Register(ITypeWrapStrategy strategy)
        {
            _strategies.Add(strategy);
        }

        public PilotObjectHelper Wrap(object obj, TypeWrapContext context)
        {
            var strategy = _strategies.FirstOrDefault(s => s.CanWrap(obj));
            if (strategy != null)
                return strategy.Wrap(obj, context);
            throw new InvalidOperationException($"No wrap strategy found for type {obj?.GetType().FullName ?? "null"}");
        }
    }
} 