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
        private PilotObjectHelper _objectHelper;
        private IRepoService _repoService;
        private ICustomSearchService _searchService;
        private IWindowService _windowService;
        private ITreeItemService _treeItemService;
        private IViewModelFactory _viewModelFactory;

        public TaskTreeVM(
             PilotObjectHelper pilotObjectHelper,
             IRepoService lookUpModel,
             ICustomSearchService searchService,
             IWindowService windowService,
             ITreeItemService treeItemService,
             IViewModelFactory viewModelFactory)
        {
            _revokedTask = false;
            _repoService = lookUpModel;
            _objectHelper = pilotObjectHelper;
            _searchService = searchService;
            _windowService = windowService;
            _treeItemService = treeItemService;
            _viewModelFactory = viewModelFactory;
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
            bool isTask;

            if (_objectHelper is DataObjectHelper dataObjectHelper)
            {
                isTask = dataObjectHelper.IsTask;
            }
            else return;

            if (isTask && _objectHelper.LookUpObject is IDataObject dataObject)
            {
                LastParrent = await _searchService.GetLastParent(dataObject);
                ICustomTree rootNode = _viewModelFactory.CreateListItemVM(LastParrent);
                rootNode = await _treeItemService.FillChild(rootNode);
                // Обновляем UI-поток
                Application.Current.Dispatcher.Invoke(() =>
                {
                    RevokedTaskVisible = Visibility.Hidden;
                    FirstParrentNode.Clear();
                    FirstParrentNode.Add(rootNode);
                });
                return;
            }
            else
            {
                var treeItems = new ObservableCollection<ICustomTree>();
                ObjectSet allLastParrent = await _searchService.GetBaseParentsOfRelations(_objectHelper, RevokedTask);
                foreach (PilotObjectHelper item in allLastParrent)
                {
                    ICustomTree rootNode = _viewModelFactory.CreateListItemVM(item);
                    rootNode = await _treeItemService.FillChild(rootNode);
                    treeItems.Add(rootNode);
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    FirstParrentNode.Clear();
                    foreach (var item in treeItems)
                    {
                        FirstParrentNode.Add(item);
                    }
                });
                return;
            }
        }
        private PilotObjectHelper LastParrent { get; set; }

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
        private void UpdateInfo()
        {
            Task.Run(async () =>
            {
                Info = await _repoService.GetObjInfo(_dataObjectSelected.PilotObjectHelper);
            });
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
