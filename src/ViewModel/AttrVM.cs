using PilotLookUp.Commands;
using PilotLookUp.Domain.Entities;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PilotLookUp.ViewModel
{
    public class AttrVM : INotifyPropertyChanged, IPage
    {
        private IPilotObjectHelper _objectHelper;
        private IDataObjectService _dataObjectService;
        private readonly IErrorHandlingService _errorHandlingService;
        private readonly IValidationService _validationService;
        private readonly ICopyDataService _copyDataService;

        public AttrVM(IPilotObjectHelper pilotObjectHelper,
            IDataObjectService dataObjectService,
            IErrorHandlingService errorHandlingService,
            IValidationService validationService,
            ICopyDataService copyDataService)
        {
            _validationService = validationService;
            _validationService.ValidateConstructorParams(pilotObjectHelper,
                dataObjectService,
                errorHandlingService,
                validationService,
                copyDataService);
            _objectHelper = pilotObjectHelper;
            _dataObjectService = dataObjectService;
            _errorHandlingService = errorHandlingService;
            _copyDataService = copyDataService;
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

        private void CopyToClipboard(object sender)
        {
            try
            {
                if (sender is CopyCommandKey key)
                {
                    switch (key)
                    {
                        case CopyCommandKey.DataGridSelectName:
                            _copyDataService.CopyAttributeName(_dataGridSelected);
                            break;
                        case CopyCommandKey.DataGridSelectValue:
                            _copyDataService.CopyAttributeValue(_dataGridSelected);
                            break;
                        case CopyCommandKey.DataGridSelectTitle:
                            _copyDataService.CopyAttributeTitle(_dataGridSelected);
                            break;
                        default:
                            _copyDataService.CopyAttributeName(_dataGridSelected);
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                _errorHandlingService?.HandleError(ex, "AttrVM.CopyToClipboard");
            }
        }

        public ICommand CopyCommand => new RelayCommand<object>(CopyToClipboard);


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        PagesName IPage.GetName() =>
            PagesName.AttrPage;
    }
}
