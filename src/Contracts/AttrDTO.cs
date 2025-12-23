using System.Collections;
using System.Linq;
using System.Windows;

namespace PilotLookUp.Contracts
{
    public class AttrDTO
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public object RawValue { get; set; }
        public string IsObligatory { get; set; }
        public string IsService { get; set; }
        public string Type { get; set; }
        public bool IsInitialized { get; set; }
        public bool IsValid { get; set; }

        public bool IsValueLinkType =>
            Type == "ElementBook"
            || Type == "UserState"
            || Type == "OrgUnit";

        public bool HasRawValue
        {
            get
            {
                if (RawValue == null) return false;
                if (RawValue is string s) return !string.IsNullOrWhiteSpace(s);
                if (RawValue is IEnumerable e && RawValue is not string)
                    return e.Cast<object>().Any(i => i != null);
                return true;
            }
        }

        public bool IsLookable => IsValueLinkType && HasRawValue;

        public string Discription => Value;

        public TextDecorationCollection Decoration =>
            IsLookable ? TextDecorations.Underline : null;
    }
}
