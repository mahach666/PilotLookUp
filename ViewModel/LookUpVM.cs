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
                    UpdateInfo();
                    OnPropertyChanged();
                }
            }
        }

        private ObjectSet _dataGridSelected;
        public ObjectSet DataGridSelected
        {
            get => _dataGridSelected;
            set
            {
                _dataGridSelected = value;
                OnPropertyChanged();
            }
        }

        private void UpdateInfo()
        {
            Task.Run(async () =>
            {
                Info = await _lookUpModel.Info(_dataObjectSelected);
            });
        }
        private List<ObjectSet> _info;
        public List<ObjectSet> Info
        {
            get => _info;
            set
            {
                _info = value;
                OnPropertyChanged();
            }
        }

        private void CopyToClipboard(string sender)
        {
            if (_dataObjectSelected == null) Clipboard.SetText("Ошибка копирования, ничего не выбрано");

            if (sender == "List")
            {
                Clipboard.SetText(_dataObjectSelected.Name);
            }
            else if (sender == "DataGridSelectName")
            {
                Clipboard.SetText(_dataGridSelected?.SenderMemberName);
            }
            else if (sender == "DataGridSelectValue")
            {
                Clipboard.SetText(_dataGridSelected?.Discription);
            }
            else if (sender == "DataGridSelectLine")
            {
                Clipboard.SetText(_dataGridSelected?.SenderMemberName + "\t" + _dataGridSelected?.Discription);
            }
            else
            {
                Clipboard.SetText("Ошибка копирования, ничего не выбрано");
            }
        }


        public ICommand CopyCommand => new RelayCommand<string>(CopyToClipboard);
        public ICommand SelectedValueCommand => new AsyncRelayCommand(_ => _lookUpModel.DataGridSelector(_dataGridSelected));




        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
