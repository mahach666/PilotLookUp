namespace PilotLookUp.ViewModel.Editors
{
    public sealed class BoolValueVM : EditorVMBase
    {
        private bool _value;
        public bool Value
        {
            get => _value;
            set { _value = value; OnPropertyChanged(); }
        }
    }
}
