using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.Objects.TypeHelpers;
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

        public TaskTreeVM(
             IPilotObjectHelper pilotObjectHelper,
             IRepoService lookUpModel,
             ICustomSearchService searchService,
             IWindowService windowService,
             ITreeItemService treeItemService,
             IErrorHandlingService errorHandlingService,
             IValidationService validationService,
             ICopyDataService copyDataService)
        {
            _validationService = validationService;
            _validationService.ValidateConstructorParams(pilotObjectHelper, lookUpModel, searchService, windowService, treeItemService, errorHandlingService, validationService, copyDataService);
            _revokedTask = false;
            _repoService = lookUpModel;
            _objectHelper = pilotObjectHelper;
            _searchService = searchService;
            _windowService = windowService;
            _treeItemService = treeItemService;
            _errorHandlingService = errorHandlingService;
            _copyDataService = copyDataService;
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
            bool isTask = false;

            // Проверяем наличие свойства IsTask через интерфейс
            var isTaskProp = _objectHelper.GetType().GetProperty("IsTask");
            if (isTaskProp != null)
            {
                isTask = (bool)isTaskProp.GetValue(_objectHelper);
            }
            else return;

            if (isTask && _objectHelper.LookUpObject is IDataObject dataObject)
            {
                LastParrent = await _searchService.GetLastParent(dataObject);
                ICustomTree rootNode = new ListItemVM(LastParrent, _validationService);
                rootNode = await _treeItemService.FillChild(rootNode);
                // Обновляем UI-поток
                Application.Current.Dispatcher.Invoke(() =>
                {
                    RevokedTaskVisible = Visibility.Hidden;
                    FirstParrentNode.Clear();
                    FirstParrentNode = new ObservableCollection<ICustomTree> { rootNode };
                });
                return;
            }
            else
            {
                var treeItems = new ObservableCollection<ICustomTree>();
                ObjectSet allLastParrent = await _searchService.GetBaseParentsOfRelations(_objectHelper, RevokedTask);
                foreach (IPilotObjectHelper item in allLastParrent)
                {
                    ICustomTree rootNode = new ListItemVM(item, _validationService);
                    rootNode = await _treeItemService.FillChild(rootNode);
                    treeItems.Add(rootNode);
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    FirstParrentNode.Clear();
                    FirstParrentNode = treeItems;
                });
                return;
            }
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

        private void CopyToClipboard(string sender)
        {
            try
            {
                switch (sender)
                {
                    case "List":
                        _copyDataService.CopyObjectName(_dataObjectSelected);
                        break;
                    case "DataGridSelectName":
                        _copyDataService.CopyMemberName(_dataGridSelected);
                        break;
                    case "DataGridSelectValue":
                        _copyDataService.CopyMemberValue(_dataGridSelected);
                        break;
                    case "DataGridSelectLine":
                        _copyDataService.CopyMemberLine(_dataGridSelected);
                        break;
                    default:
                        _copyDataService.CopyObjectName(_dataObjectSelected);
                        break;
                }
            }
            catch (System.Exception ex)
            {
                _errorHandlingService?.HandleError(ex, "TaskTreeVM.CopyToClipboard");
            }
        }

        public ICommand CopyCommand => new RelayCommand<string>(CopyToClipboard);
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
