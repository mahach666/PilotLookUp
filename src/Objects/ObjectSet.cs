using PilotLookUp.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;

namespace PilotLookUp.Objects
{
    public class ObjectSet : List<PilotObjectHelper>
    {
        private readonly IThemeService _themeService;
        private MemberInfo _memberInfo;
        public ObjectSet(IThemeService themeService, MemberInfo memberInfo)
        {
            _themeService = themeService;
            _memberInfo = memberInfo;
        }

        public bool IsMethodResult { get => _memberInfo?.MemberType is MemberTypes.Method; }
        public string SenderMemberName { get => IsMethodResult ? _memberInfo?.Name + "()" : _memberInfo?.Name; }

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

        public bool IsLookable
        {
            get
            {
                if (!this.Any()) return false;
                else if (this.FirstOrDefault()?.IsLookable == true)
                {
                    return true;
                }
                return false;
            }
        }

        public override string ToString()
        {
            return Discription;
        }
    }
}
