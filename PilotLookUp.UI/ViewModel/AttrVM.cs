using PilotLookUp.Commands;
using PilotLookUp.Contracts;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Core.Objects;
using PilotLookUp.Core.Objects.TypeHelpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace PilotLookUp.UI.ViewModel
{
    public class AttrVM : INotifyPropertyChanged, IPage
    {
        private PilotObjectHelper _objectHelper;
        private IDataObjectService _dataObjectService;
        private readonly IClipboardService _clipboard;

        public AttrVM(PilotObjectHelper pilotObjectHelper, IDataObjectService dataObjectService, IClipboardService clipboardService)
        {
            _objectHelper = pilotObjectHelper;
            _dataObjectService = dataObjectService;
            _clipboard = clipboardService;
        }

        public IEnumerable<AttrDTO> Attrs
        {
            get
            {
                if (_objectHelper == null || _objectHelper.LookUpObject == null)
                    return new List<AttrDTO>();
                return _dataObjectService.GetAttrDTOs(_objectHelper as DataObjectHelper);
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
                    _clipboard.SetText(_dataGridSelected?.Name);
                }
                else if (sender == "DataGridSelectValue")
                {
                    _clipboard.SetText(_dataGridSelected?.Value);
                }
                else if (sender == "DataGridSelectTitle")
                {
                    _clipboard.SetText(_dataGridSelected?.Title);
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


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        PagesName IPage.GetName() =>
            PagesName.AttrPage;
    }
}
