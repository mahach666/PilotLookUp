using PilotLookUp.Commands;
using PilotLookUp.Contracts;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects.TypeHelpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace PilotLookUp.ViewModel
{
    public class AttrVM : INotifyPropertyChanged, IPage
    {
        private IPilotObjectHelper _objectHelper;
        private IDataObjectService _dataObjectService;
        private readonly IErrorHandlingService _errorHandlingService;

        public AttrVM(IPilotObjectHelper pilotObjectHelper, IDataObjectService dataObjectService, IErrorHandlingService errorHandlingService)
        {
            _objectHelper = pilotObjectHelper;
            _dataObjectService = dataObjectService;
            _errorHandlingService = errorHandlingService;
        }

        public IEnumerable<AttrDTO> Attrs
        {
            get
            {
                if (_objectHelper == null || _objectHelper.LookUpObject == null)
                    return new List<AttrDTO>();
                return _dataObjectService.GetAttrDTOs(_objectHelper);
            }
        }

        public string IdSelectedItem
        {
            get
            {
                return _objectHelper.StringId;
            }
        }
        public string NameSelectedItem
        {
            get
            {
                return _objectHelper.Name;
            }
        }

        private AttrDTO _dataGridSelected;
        public AttrDTO DataGridSelected
        {
            get => _dataGridSelected;
            set
            {
                _dataGridSelected = value;
                OnPropertyChanged();
            }
        }

        private void CopyToClipboard(string sender)
        {
            var errorText = "Упс, ничего не выбрано.";
            if (_dataGridSelected == null) MessageBox.Show(errorText);
            try
            {
             if (sender == "DataGridSelectName")
                {
                    Clipboard.SetText(_dataGridSelected?.Name);
                }
                else if (sender == "DataGridSelectValue")
                {
                    Clipboard.SetText(_dataGridSelected?.Value);
                }
                else if (sender == "DataGridSelectTitle")
                {
                    Clipboard.SetText(_dataGridSelected?.Title);
                }
                else
                {
                    MessageBox.Show(errorText);
                }
            }
            catch (System.Exception ex)
            {
                _errorHandlingService?.HandleError(ex, "AttrVM.CopyToClipboard");
                MessageBox.Show(errorText);
            }
        }

        public ICommand CopyCommand => new RelayCommand<string>(CopyToClipboard);


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        PagesName IPage.GetName() =>
            PagesName.AttrPage;
    }
}
