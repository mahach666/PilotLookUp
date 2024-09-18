using Ascon.Pilot.SDK;
using PilotLookUp.Commands;
using PilotLookUp.Model;
using PilotLookUp.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PilotLookUp.VM
{
    internal class LookUpVM : INotifyPropertyChanged
    {
        internal LookUpView _view;
        private LookUpModel _lookUpModel;
        private PilotTypsHelper _dataObjectSelected;
        private ICommand _selectedValueClickCommand;

        public LookUpVM(LookUpModel lookUpModel)
        {
            _lookUpModel = lookUpModel;
            DataObjectSelected = SelectionDataObjects.FirstOrDefault();
            _selectedValueClickCommand = new AsyncRelayCommand(_lookUpModel.DataGridSelector);
        }

        public List<PilotTypsHelper> SelectionDataObjects => _lookUpModel.SelectionDataObjects;

        public PilotTypsHelper DataObjectSelected
        {
            get => _dataObjectSelected;
            set
            {
                if (_dataObjectSelected != value)
                {
                    _dataObjectSelected = value;
                    OnPropertyChanged("Info");
                    OnPropertyChanged();
                }
            }
        }

        public ObjReflection Info => _lookUpModel.GetInfo(_dataObjectSelected);

        public ICommand SelectedValueClickCommand => _selectedValueClickCommand;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
