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
        private readonly List<PilotObjectHelper> _items = new List<PilotObjectHelper>();
        private readonly IThemeService _themeService;
        private readonly MemberInfo _memberInfo;
        public ObjectSet(IThemeService themeService, MemberInfo memberInfo)
        {
            _themeService = themeService;
            _memberInfo = memberInfo;
        }

        public int Count => _items.Count;
        public PilotObjectHelper this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }
        public MemberInfo MemberInfo => _memberInfo;

        public void Add(PilotObjectHelper item) => _items.Add(item);
        public void AddRange(IEnumerable<PilotObjectHelper> items) => _items.AddRange(items);
        public bool Remove(PilotObjectHelper item) => _items.Remove(item);
        public void Clear() => _items.Clear();
        public bool Contains(PilotObjectHelper item) => _items.Contains(item);
        public IEnumerator<PilotObjectHelper> GetEnumerator() => _items.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _items.GetEnumerator();

        public bool IsMethodResult => _memberInfo?.MemberType is MemberTypes.Method;
        public string SenderMemberName => IsMethodResult ? _memberInfo?.Name + "()" : _memberInfo?.Name;

        public string Discription
        {
            get
            {
                var firstObj = this.FirstOrDefault();
                if (Count == 0) return "No objects";
                else if (Count == 1)
                {
                    return firstObj?.StringId ?? firstObj?.Name ?? firstObj?.ToString() ?? "Ошибка вычисления";
                }
                else return $"List<{firstObj?.LookUpObject?.GetType().Name: invalid}>Count = {Count}";
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
