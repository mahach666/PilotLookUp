﻿using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PilotLookUp.ViewModel
{
    public class LookUpVM : INotifyPropertyChanged, IPage
    {
        private IRepoService _lookUpModel;
        private IWindowService _windowService;

        public LookUpVM(IRepoService lookUpModel, IWindowService windowService)
        {
            _lookUpModel = lookUpModel;
            _windowService = windowService;
            DataObjectSelected = SelectionDataObjects?.FirstOrDefault();
        }

        private List<ListItemVM> _selectionDataObjects;
        public List<ListItemVM> SelectionDataObjects
        {
            get => _selectionDataObjects;
            set
            {
                if (value == null || !value.Any()) return;
                _selectionDataObjects = value;
                DataObjectSelected = value?.FirstOrDefault();
                UpdateInfo();
                OnPropertyChanged();
            }
        }

        private ListItemVM _dataObjectSelected;
        public ListItemVM DataObjectSelected
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
                Info = await _lookUpModel.GetObjInfo(_dataObjectSelected.PilotObjectHelper);
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
            var errorText = "Упс, ничего не выбрано.";
            if (_dataObjectSelected == null) MessageBox.Show(errorText);

            try
            {
                if (sender == "List")
                {
                    Clipboard.SetText(_dataObjectSelected.PilotObjectHelper?.Name);
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
                    MessageBox.Show(errorText);
                }
            }
            catch
            {
                MessageBox.Show(errorText);
            }
        }

        public ICommand CopyCommand => new RelayCommand<string>(CopyToClipboard);
        public ICommand SelectedValueCommand => new RelayCommand<object>(_ => _windowService.CreateNewMainWindiw(_dataGridSelected));


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        PagesName IPage.GetName()
        {
            return PagesName.LookUpPage;
        }
    }
}
