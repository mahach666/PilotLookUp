using System.Windows;

namespace PilotLookUp.ViewModel.Editors
{
    // Число: одно значение (Равно) или диапазон (От/До)
    public sealed class NumberRangeVM : EditorVMBase
    {
        public NumberRangeVM(bool isRange)
        {
            IsRange = isRange;
        }

        public bool IsRange { get; }
        public Visibility ToVisibility => IsRange ? Visibility.Visible : Visibility.Collapsed;

        private string _from;
        public string From
        {
            get => _from;
            set { _from = value; OnPropertyChanged(); }
        }

        private string _to;
        public string To
        {
            get => _to;
            set { _to = value; OnPropertyChanged(); }
        }
    }
}
