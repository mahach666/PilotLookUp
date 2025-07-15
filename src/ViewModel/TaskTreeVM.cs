using PilotLookUp.Commands;
using PilotLookUp.Domain.Entities;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Utils;
using PilotLookUp.Model.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using IDataObject = Ascon.Pilot.SDK.IDataObject;


namespace PilotLookUp.ViewModel
{
    public class TaskTreeVM : INotifyPropertyChanged, IPage
    {
        private IPilotObjectHelper _objectHelper;
        private IRepoService _repoService;
        private ICustomSearchService _searchService;
        private IWindowService _windowService;
        private ITreeItemService _treeItemService;
        private readonly IErrorHandlingService _errorHandlingService;
        private readonly IValidationService _validationService;
        private readonly ICopyDataService _copyDataService;
        private readonly TaskTreeBuilderService _taskTreeBuilderService;

        public TaskTreeVM(
             IPilotObjectHelper pilotObjectHelper,
             IRepoService lookUpModel,
             ICustomSearchService searchService,
             IWindowService windowService,
             ITreeItemService treeItemService,
             IErrorHandlingService errorHandlingService,
             IValidationService validationService,
             ICopyDataService copyDataService,
             TaskTreeBuilderService taskTreeBuilderService)
        {
            _validationService = validationService;
            _validationService.ValidateConstructorParams(pilotObjectHelper,
                lookUpModel,
                searchService,
                windowService,
                treeItemService,
                errorHandlingService,
                validationService,
                copyDataService,
                taskTreeBuilderService);
            _revokedTask = false;
            _repoService = lookUpModel;
            _objectHelper = pilotObjectHelper;
            _searchService = searchService;
            _windowService = windowService;
            _treeItemService = treeItemService;
            _errorHandlingService = errorHandlingService;
            _copyDataService = copyDataService;
            _taskTreeBuilderService = taskTreeBuilderService;
            FirstParrentNode = new ObservableCollection<ICustomTree>();
            _ = LoadDataAsync();
        }

        #region Информация о выбранном задании
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


        private bool _revokedTask;
        public bool RevokedTask
        {

            get => _revokedTask;
            set
            {
                _revokedTask = value;
                _ = LoadDataAsync();
                OnPropertyChanged();
            }
        }

        private Visibility _revokedTaskVisible;
        public Visibility RevokedTaskVisible
        {

            get => _revokedTaskVisible;
            set
            {
                _revokedTaskVisible = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Дерево процесса
        private async Task LoadDataAsync()
        {
            var (nodes, revokedTaskVisible, lastParent) = await _taskTreeBuilderService.BuildTaskTreeAsync(_objectHelper, RevokedTask);
            Application.Current.Dispatcher.Invoke(() =>
            {
                RevokedTaskVisible = revokedTaskVisible;
                FirstParrentNode = nodes;
                LastParrent = lastParent;
            });
        }
        private IPilotObjectHelper LastParrent { get; set; }

        private ObservableCollection<ICustomTree> _firstParrentNode;

        public ObservableCollection<ICustomTree> FirstParrentNode
        {

            get => _firstParrentNode;
            set
            {
                _firstParrentNode = value;
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
        private async void UpdateInfo()
        {
            if (_dataObjectSelected == null || _dataObjectSelected.PilotObjectHelper == null)
            {
                Info = null;
                return;
            }
            Info = await _repoService.GetObjInfo(_dataObjectSelected.PilotObjectHelper);
        }

        #endregion

        #region Свойства выбранного объекта
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
        public ICommand SelectedValueCommand => new RelayCommand<object>(_ => _windowService.CreateNewMainWindow(_dataGridSelected));

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
                _errorHandlingService?.HandleError(ex, "TaskTreeVM.CopyToClipboard");
            }
        }
        public ICommand CopyCommand => new RelayCommand<object>(CopyToClipboard);
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        PagesName IPage.GetName() =>
             PagesName.TaskTree;
    }
}
