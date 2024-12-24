using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;

namespace PilotLookUp.Objects
{
    public class ObjectSet : List<PilotObjectHelper>
    {
        public ObjectSet(MemberInfo memberInfo)
        {
            _memberInfo = memberInfo;
        }
        public ObjectSet(IEnumerable<PilotObjectHelper> collection)
        {
            foreach (PilotObjectHelper item in collection)
            {
                Add(item);
            }
        }
        private MemberInfo _memberInfo { get; }
        public bool IsMethodResult { get => _memberInfo.MemberType is MemberTypes.Method; }
        public string SenderMemberName { get => IsMethodResult ? _memberInfo.Name + "()" : _memberInfo.Name; }

        public string Discription
        {
            get
            {
                var firstObj = this.FirstOrDefault();
                if (Count == 0) return "No objects";
                else if (Count == 1)
                {
                    if (SenderMemberName.EndsWith("Id")) return firstObj?.StringId ?? firstObj?.Name ?? "Ошибка вычисления";
                    return firstObj?.Name ?? "NULL";
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
                    return new SolidColorBrush(Colors.Blue);
                }
                return new SolidColorBrush(Colors.Black);
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
