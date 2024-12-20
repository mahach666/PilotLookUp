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
using System.Windows;
using System.Windows.Input;

namespace PilotLookUp.ViewModel
{
    internal class LookUpVM : INotifyPropertyChanged
    {
        internal LookUpView _view;
        private LookUpModel _lookUpModel;
        private ICommand _selectedValueClickCommand;
        //public ICommand CopyCommand { get; }

        public LookUpVM(LookUpModel lookUpModel)
        {
            _lookUpModel = lookUpModel;
            DataObjectSelected = SelectionDataObjects.FirstOrDefault();
            _selectedValueClickCommand = new AsyncRelayCommand(_lookUpModel.DataGridSelector);
            //CopyCommand = new RelayCommand<string>(CopyToClipboard);
        }

        public List<PilotObjectHelper> SelectionDataObjects => _lookUpModel.SelectionDataObjects;

        private PilotObjectHelper _dataObjectSelected;
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

        private KeyValuePair<string, object> _dataGridSelected;
        public KeyValuePair<string, object> DataGridSelected
        {
            get => _dataGridSelected;
            set
            {
                _dataGridSelected = value;
                OnPropertyChanged();
            }
        }

        public ObjReflection Info => _lookUpModel.GetInfo(_dataObjectSelected);

        private void CopyToClipboard(string text)
        {
            if (text == "List")
            {
                Clipboard.SetText(_dataObjectSelected.Name);
            }
            else if (text == "DataGridSelectName")
            {
                Clipboard.SetText(_dataGridSelected.Key);
            }
            else if (text == "DataGridSelectValue")
            {
                Clipboard.SetText(_dataGridSelected.Value.ToString());
            }
            else if (text == "DataGridSelectLine")
            {
                Clipboard.SetText(_dataGridSelected.Key + "\t" + _dataGridSelected.Value.ToString());
            }
        }
        public ICommand CopyCommand => new RelayCommand<string>(CopyToClipboard);

        public ICommand SelectedValueClickCommand => _selectedValueClickCommand;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
