using PilotLookUp.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PilotLookUp.ViewModel
{
    internal class SearchVM : INotifyPropertyChanged
    {
        private LookUpModel _lookUpModel { get; }

        public SearchVM(LookUpModel lookUpModel)
        {
            _lookUpModel = lookUpModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
