using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Objects;
using PilotLookUp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PilotLookUp.ViewModel
{
    internal class CastomObjBoxVM : INotifyPropertyChanged
    {
        private PilotObjectHelper _dataObj;
        private MainVM _mainVM { get; }

        internal CastomObjBoxVM(PilotObjectHelper pilotObjectHelper, MainVM mainVM)
        {
            _dataObj = pilotObjectHelper;
            _mainVM = mainVM;
        }

        public string Name => _dataObj.Name;
        public string Id => _dataObj.StringId;

        private void GoPage()
        {
            _mainVM.ChangePage(_dataObj);
        }

        public ICommand GoPageCommand => new RelayCommand<object>(_ => GoPage());

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
