using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;

namespace PilotLookUp.Domain.Entities
{
    public class ObjectSet : IObjectSet
    {
        private readonly List<IPilotObjectHelper> _items = new List<IPilotObjectHelper>();
        private readonly IThemeService _themeService;
        private readonly IValidationService _validationService;
        private readonly MemberInfo _memberInfo;
        private readonly ILogger _logger;

        public ObjectSet(IThemeService themeService,
            MemberInfo memberInfo,
            IValidationService validationService,
            ILogger logger)
        {
            _validationService = validationService;
            _validationService.ValidateConstructorParams(themeService, validationService);
            _themeService = themeService;
            _memberInfo = memberInfo;
            _logger = logger;
        }

        public int Count => _items.Count;
        public IPilotObjectHelper this[int index]
        {
            get
            {
                _validationService.ValidateNotNull(index, nameof(index));
                if (index < 0 || index >= _items.Count)
                {
                    _logger.Warn($"Индекс вне диапазона: {index}");
                    return null;
                }
                var obj = _items[index];
                if (obj == null)
                {
                    _logger.Warn($"Обнаружен null по индексу {index}");
                }
                return obj;
            }
            set
            {
                _validationService.ValidateNotNull(index, nameof(index));
                if (index < 0 || index >= _items.Count)
                {
                    _logger.Warn($"Индекс вне диапазона при set: {index}");
                    return;
                }
                if (value == null)
                {
                    _logger.Warn($"Попытка установить null по индексу {index}");
                    return;
                }
                _items[index] = value;
            }
        }
        public MemberInfo MemberInfo => _memberInfo;

        public void Add(IPilotObjectHelper item)
        {
            _validationService.ValidateNotNull(item, nameof(item));
            if (item == null)
            {
                _logger.Warn("Попытка добавить null");
                return;
            }
            _items.Add(item);
        }
        public void AddRange(IEnumerable<IPilotObjectHelper> items)
        {
            _validationService.ValidateNotNull(items, nameof(items));
            if (items == null) return;
            foreach (var item in items)
            {
                if (item == null)
                {
                    _logger.Warn("Попытка добавить null в AddRange");
                    continue;
                }
                _items.Add(item);
            }
        }
        public bool Remove(IPilotObjectHelper item)
        {
            _validationService.ValidateNotNull(item, nameof(item));
            return _items.Remove(item);
        }
        public void Clear() => _items.Clear();
        public bool Contains(IPilotObjectHelper item)
        {
            _validationService.ValidateNotNull(item, nameof(item));
            if (item == null) return false;
            return _items.Contains(item);
        }
        public IEnumerator<IPilotObjectHelper> GetEnumerator() => _items.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _items.GetEnumerator();

        public bool IsMethodResult => _memberInfo?.MemberType is MemberTypes.Method;
        public string SenderMemberName => IsMethodResult ? _memberInfo?.Name + "()" : _memberInfo?.Name;

        public string Discription
        {
            get
            {
                var firstObj = this.FirstOrDefault(x => x != null);
                if (Count == 0) return PilotLookUp.Resources.Strings.NoObjects;
                else if (firstObj == null) return PilotLookUp.Resources.Strings.AllObjectsNull;
                else if (Count == 1)
                {
                    return firstObj?.StringId ?? firstObj?.Name ?? firstObj?.ToString() ?? PilotLookUp.Resources.Strings.CalculationError;
                }
                else return $"List<{firstObj?.LookUpObject?.GetType().Name ?? "invalid"}>Count = {Count}";
            }
        }

        public Brush Color
        {
            get
            {
                if (IsLookable == true)
                {
                    return new SolidColorBrush(_themeService.IsJediTheme ? Colors.Blue : Colors.SteelBlue);
                }
                else if (Discription.StartsWith("Error:")) return new SolidColorBrush(Colors.Red);
                return new SolidColorBrush(_themeService.IsJediTheme ? Colors.Black : Colors.White);
            }
        }

        public TextDecorationCollection Decoration
        {
            get
            {
                if (IsLookable == true)
                {
                    return TextDecorations.Underline;
                }
                return null;
            }
        }

        public bool IsLookable => this.Any(x => x.IsLookable);

        public override string ToString()
        {
            return Discription;
        }
    }
}
