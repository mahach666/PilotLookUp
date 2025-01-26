using Ascon.Pilot.SDK;
using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Model;
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
        private LookUpModel _lookUpModel;

        private PilotObjectHelper _dataObj;
        private MainVM _mainVM { get; }

        internal CastomObjBoxVM(LookUpModel lookUpModel, PilotObjectHelper pilotObjectHelper, MainVM mainVM)
        {
            _lookUpModel = lookUpModel;
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

        private void GoObj()
        {
            if (_dataObj.LookUpObject is IDataObject dataObj)
                _lookUpModel.GoTo(dataObj);
        }

        public ICommand GoObjCommand => new RelayCommand<object>(_ => GoObj());

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
