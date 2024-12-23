using PilotLookUp.Commands;
using PilotLookUp.Core;
using PilotLookUp.Model;
using PilotLookUp.Objects;
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

        public LookUpVM(LookUpModel lookUpModel)
        {
            _lookUpModel = lookUpModel;
            DataObjectSelected = SelectionDataObjects.FirstOrDefault();
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
                    Task.Run(async () =>
                    {
                        Info = await _lookUpModel.Info(_dataObjectSelected);
                        OnPropertyChanged("Info");
                    }
                    );
                    OnPropertyChanged();
                }
            }
        }

        private KeyValuePair<string, ObjectSet> _dataGridSelected;
        public KeyValuePair<string, ObjectSet> DataGridSelected
        {
            get => _dataGridSelected;
            set
            {
                _dataGridSelected = value;
                OnPropertyChanged("Info");
                OnPropertyChanged();
            }
        }

        public Dictionary<string, ObjectSet> Info { get; set; }

        private void CopyToClipboard(string sender)
        {
            if (sender == "List" && _dataObjectSelected != null)
            {
                Clipboard.SetText(_dataObjectSelected.Name);
            }
            else if (sender == "DataGridSelectName" && _dataGridSelected.Key != null)
            {
                Clipboard.SetText(_dataGridSelected.Key);
            }
            else if (sender == "DataGridSelectValue" && _dataGridSelected.Value.Discription != null)
            {
                Clipboard.SetText(_dataGridSelected.Value.ToString());
            }
            else if (sender == "DataGridSelectLine" && _dataGridSelected.Key != null)
            {
                Clipboard.SetText(_dataGridSelected.Key + "\t" + _dataGridSelected.Value.Discription);
            }
            else
            {
                Clipboard.SetText("Ошибка копирования, ничего не выбрано");
            }
        }


        public ICommand CopyCommand => new RelayCommand<string>(CopyToClipboard);
        public ICommand SelectedValueClickCommand => new AsyncRelayCommand(_ => _lookUpModel.DataGridSelector( _dataGridSelected.Value));




        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
