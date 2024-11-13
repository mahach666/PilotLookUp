using Ascon.Pilot.SDK;
using PilotLookUp.Commands;
using PilotLookUp.Core;
using PilotLookUp.Model;
using PilotLookUp.Objects;
using PilotLookUp.Objects.TypeHelpers;
using PilotLookUp.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PilotLookUp.ViewModel
{
    internal class LookUpVM : INotifyPropertyChanged
    {
        internal LookUpView _view;
        private LookUpModel _lookUpModel;
        private PilotObjectHelper _dataObjectSelected;
        public ICommand SelectedValueClickCommand => new AsyncRelayCommand(_lookUpModel.DataGridSelector);
        public ICommand CopyCommand => new RelayCommand(CopyToMem);
        //public ICommand CopyCommand => new AsyncRelayCommand(CopyToMem);


        public LookUpVM(LookUpModel lookUpModel)
        {
            _lookUpModel = lookUpModel;
            DataObjectSelected = SelectionDataObjects.FirstOrDefault();
            //_selectedValueClickCommand = new AsyncRelayCommand(_lookUpModel.DataGridSelector);
        }

        public List<PilotObjectHelper> SelectionDataObjects => _lookUpModel.SelectionDataObjects;

        public PilotObjectHelper DataObjectSelected
        {
            get => _dataObjectSelected;
            set
            {
                if (_dataObjectSelected != value)
                {
                    _dataObjectSelected = value;
                    _lookUpModel.Update(value);
                    OnPropertyChanged("Info");
                    OnPropertyChanged();
                }
            }
        }

        private void CopyToMem(object value)
        {
            if (value != null)
            {
                Clipboard.SetText(value.ToString());
            }
        }
        //private async Task CopyToMem(object value)
        //{
        //    if (value != null)
        //    {
        //        Clipboard.SetText(value.ToString());
        //    }
        //}

        public ObjReflection Info => _lookUpModel.GetInfo(_dataObjectSelected);

        //public ICommand SelectedValueClickCommand => _selectedValueClickCommand;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
