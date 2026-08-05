using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Model.AdvancedSearch
{
    // Один атрибут (имя + тип) в том виде, как он определен во всех типах базы.
    // Поиск идет по имени атрибута, но заголовок и настройки у типов могут отличаться:
    // grif в 783 типах называется «Гриф», а в 24 - «Гриф конфиденциальности»
    public sealed class AttributeGroup
    {
        private readonly Dictionary<string, int> _titleCounts = new Dictionary<string, int>();
        private readonly List<string> _configurations = new List<string>();

        public AttributeGroup(string name, AttributeType type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public AttributeType Type { get; }

        public void Add(IAttribute attribute)
        {
            var title = attribute.Title ?? string.Empty;
            _titleCounts.TryGetValue(title, out var count);
            _titleCounts[title] = count + 1;

            if (!string.IsNullOrWhiteSpace(attribute.Configuration) && !_configurations.Contains(attribute.Configuration))
                _configurations.Add(attribute.Configuration);
        }

        // Заголовок, которым атрибут назван в наибольшем числе типов
        public string MainTitle => _titleCounts
            .OrderByDescending(p => p.Value)
            .ThenBy(p => p.Key, StringComparer.CurrentCultureIgnoreCase)
            .Select(p => p.Key)
            .FirstOrDefault() ?? string.Empty;

        // Все встречающиеся заголовки, самый частый - первым
        public IReadOnlyList<string> Titles => _titleCounts
            .OrderByDescending(p => p.Value)
            .ThenBy(p => p.Key, StringComparer.CurrentCultureIgnoreCase)
            .Select(p => p.Key)
            .Where(t => t.Length > 0)
            .ToList();

        // Значения Configuration по всем типам: у атрибута-состояния это id
        // машины состояний, и он может отличаться от типа к типу
        public IReadOnlyList<string> Configurations => _configurations;
    }
}
