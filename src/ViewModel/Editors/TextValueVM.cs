namespace PilotLookUp.ViewModel.Editors
{
    public sealed class TextValueVM : EditorVMBase
    {
        public TextValueVM(string hint)
        {
            Hint = hint;
        }

        public string Hint { get; }

        private string _text;
        public string Text
        {
            get => _text;
            set { _text = value; OnPropertyChanged(); }
        }
    }
}
