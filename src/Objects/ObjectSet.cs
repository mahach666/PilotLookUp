using PilotLookUp.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;

namespace PilotLookUp.Objects
{
    public class ObjectSet : IObjectSet
    {
        private readonly List<IPilotObjectHelper> _items = new List<IPilotObjectHelper>();
        private readonly IThemeService _themeService;
        private readonly MemberInfo _memberInfo;
        public ObjectSet(IThemeService themeService, MemberInfo memberInfo)
        {
            _themeService = themeService;
            _memberInfo = memberInfo;
        }

        public int Count => _items.Count;
        public IPilotObjectHelper this[int index]
        {
            get
            {
                if (index < 0 || index >= _items.Count)
                {
                    System.Diagnostics.Debug.WriteLine($"[ObjectSet] Индекс вне диапазона: {index}");
                    return null;
                }
                var obj = _items[index];
                if (obj == null)
                {
                    System.Diagnostics.Debug.WriteLine($"[ObjectSet] Обнаружен null по индексу {index}");
                }
                return obj;
            }
            set
            {
                if (index < 0 || index >= _items.Count)
                {
                    System.Diagnostics.Debug.WriteLine($"[ObjectSet] Индекс вне диапазона при set: {index}");
                    return;
                }
                if (value == null)
                {
                    System.Diagnostics.Debug.WriteLine($"[ObjectSet] Попытка установить null по индексу {index}");
                    return;
                }
                _items[index] = value;
            }
        }
        public MemberInfo MemberInfo => _memberInfo;

        public void Add(IPilotObjectHelper item)
        {
            if (item == null)
            {
                System.Diagnostics.Debug.WriteLine("[ObjectSet] Попытка добавить null");
                return;
            }
            _items.Add(item);
        }
        public void AddRange(IEnumerable<IPilotObjectHelper> items)
        {
            if (items == null) return;
            foreach (var item in items)
            {
                if (item == null)
                {
                    System.Diagnostics.Debug.WriteLine("[ObjectSet] Попытка добавить null в AddRange");
                    continue;
                }
                _items.Add(item);
            }
        }
        public bool Remove(IPilotObjectHelper item) => _items.Remove(item);
        public void Clear() => _items.Clear();
        public bool Contains(IPilotObjectHelper item)
        {
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
                if (Count == 0) return "No objects";
                else if (firstObj == null) return "[ObjectSet] Все объекты null";
                else if (Count == 1)
                {
                    return firstObj?.StringId ?? firstObj?.Name ?? firstObj?.ToString() ?? "Ошибка вычисления";
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
