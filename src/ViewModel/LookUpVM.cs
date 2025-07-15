using PilotLookUp.Commands;
using PilotLookUp.Domain.Entities;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Enums;
using PilotLookUp.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Application = System.Windows.Application;

namespace PilotLookUp.ViewModel
{
    public class LookUpVM : BaseValidatedViewModel, IPage
    {
        private IRepoService _repoService;
        private IWindowService _windowService;
        private bool _dataInitialized = false;
        private readonly IErrorHandlingService _errorHandlingService;
        private readonly IDataInitializationService _dataInitializationService;
        private readonly IDataFilterService _dataFilterService;
        private readonly ICopyDataService _copyDataService;
        private readonly ILogger _logger;

        public LookUpVM(IRepoService lookUpModel,
            IWindowService windowService,
            IErrorHandlingService errorHandlingService,
            IValidationService validationService,
            IDataInitializationService dataInitializationService,
            IDataFilterService dataFilterService,
            ICopyDataService copyDataService,
            ILogger logger,
            List<ListItemVM> initialData = null)
            : base(validationService,
                  lookUpModel,
                  windowService,
                  errorHandlingService,
                  validationService,
                  dataInitializationService,
                  dataFilterService,
                  copyDataService)
        {
            _repoService = lookUpModel;
            _windowService = windowService;
            _errorHandlingService = errorHandlingService;
            _dataInitializationService = dataInitializationService;
            _dataFilterService = dataFilterService;
            _copyDataService = copyDataService;
            _logger = logger;

            if (initialData != null && initialData.Count > 0)
            {
                SelectionDataObjects = initialData;
            }
            else
            {
                _ = InitializeDataAsync();
            }
        }

        private async Task InitializeDataAsync()
        {
            try
            {
                var listItems = await _dataInitializationService.InitializeDataFromRepositoryAsync();
                if (_dataInitializationService.ValidateData(listItems))
                {
                    SelectionDataObjects = listItems;
                }
                else
                {
                    SelectionDataObjects = new List<ListItemVM>();
                }
            }
            catch (System.Exception ex)
            {
                _errorHandlingService?.HandleError(ex, "LookUpVM.InitializeDataAsync");
            }
        }

        private List<ListItemVM> _selectionDataObjects;
        public List<ListItemVM> SelectionDataObjects
        {
            get => _selectionDataObjects;
            set
            {
                if (value != null && value.Count > 0)
                {
                    _selectionDataObjects = value;
                    _dataInitialized = true;
                    DataObjectSelected = _selectionDataObjects?.FirstOrDefault();
                    OnPropertyChanged();
                    _ = UpdateFiltredDataObjectsAsync();
                }
                else if (!_dataInitialized)
                {
                    _ = InitializeDataAsync();
                }
            }
        }

        public void InitializeDataIfNeeded()
        {
            if (!_dataInitialized)
            {
                _ = InitializeDataAsync();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PromtVisibility));
                _ = UpdateFiltredDataObjectsAsync();
            }
        }

        public Visibility PromtVisibility => string.IsNullOrEmpty(_searchText) ? Visibility.Visible : Visibility.Hidden;

        private List<ListItemVM> _filtredDataObjects;
        public List<ListItemVM> FiltredDataObjects
        {
            get => _filtredDataObjects;
            private set
            {
                _filtredDataObjects = value;
                OnPropertyChanged();
            }
        }

        public async Task UpdateFiltredDataObjectsAsync()
        {
            try
            {
                var filtered = await _dataFilterService.FilterDataAsync(SelectionDataObjects, SearchText);
                await Application.Current.Dispatcher.InvokeAsync(() => FiltredDataObjects = filtered);
            }
            catch (System.Exception ex)
            {
                _errorHandlingService?.HandleError(ex, "LookUpVM.UpdateFiltredDataObjectsAsync");
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

        private async void UpdateInfo()
        {
            if (_dataObjectSelected == null || _dataObjectSelected.PilotObjectHelper == null)
            {
                Info = null;
                return;
            }
            Info = await _repoService.GetObjInfo(_dataObjectSelected.PilotObjectHelper);
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

        private void CopyToClipboard(object sender)
        {
            try
            {
                if (sender is CopyCommandKey key)
                {
                    switch (key)
                    {
                        case CopyCommandKey.List:
                            _copyDataService.CopyObjectName(_dataObjectSelected);
                            break;
                        case CopyCommandKey.DataGridSelectName:
                            _copyDataService.CopyMemberName(_dataGridSelected);
                            break;
                        case CopyCommandKey.DataGridSelectValue:
                            _copyDataService.CopyMemberValue(_dataGridSelected);
                            break;
                        case CopyCommandKey.DataGridSelectLine:
                            _copyDataService.CopyMemberLine(_dataGridSelected);
                            break;
                        default:
                            _copyDataService.CopyObjectName(_dataObjectSelected);
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                _errorHandlingService?.HandleError(ex, "LookUpVM.CopyToClipboard");
            }
        }
        public ICommand CopyCommand => new RelayCommand<object>(CopyToClipboard);
        public ICommand SelectedValueCommand => new RelayCommand<object>(_ => _windowService.CreateNewMainWindow(_dataGridSelected));

        PagesName IPage.GetName() =>
             PagesName.LookUpPage;
    }
}
