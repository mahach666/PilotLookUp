using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PilotLookUp.ViewModel.Editors
{
    public abstract class EditorVMBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
