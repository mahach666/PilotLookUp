using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.Objects.TypeHelpers;
using System;
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
        private IViewFactory _viewFactory;
        private ITreeItemService _treeItemService;
        private IViewModelFactory _viewModelFactory;

        public TaskTreeVM(
             PilotObjectHelper pilotObjectHelper,
             IRepoService lookUpModel,
             ICustomSearchService searchService,
             IViewFactory viewFactory,
             ITreeItemService treeItemService,
             IViewModelFactory viewModelFactory)
        {
            _revokedTask = false;
            _repoService = lookUpModel;
            _objectHelper = pilotObjectHelper;
            _searchService = searchService;
            _viewFactory = viewFactory;
            _treeItemService = treeItemService;
            _viewModelFactory = viewModelFactory;
            FirstParentNode = new ObservableCollection<ICustomTree>();
            LoadDataSafe();
        }

        private async void LoadDataSafe()
        {
            try
            {
                await LoadDataAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
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
                LoadDataSafe();
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
                LastParent = await _searchService.GetLastParent(dataObject);
                ICustomTree rootNode = _viewModelFactory.CreateListItemVM(LastParent);
                rootNode = await _treeItemService.FillChild(rootNode);
                // Обновляем UI-поток
                Application.Current.Dispatcher.Invoke(() =>
                {
                    RevokedTaskVisible = Visibility.Hidden;
                    FirstParentNode.Clear();
                    FirstParentNode.Add(rootNode);
                });
                return;
            }
            else
            {
                var treeItems = new ObservableCollection<ICustomTree>();
                ObjectSet allLastParent = await _searchService.GetBaseParentsOfRelations(_objectHelper, RevokedTask);
                foreach (PilotObjectHelper item in allLastParent)
                {
                    ICustomTree rootNode = _viewModelFactory.CreateListItemVM(item);
                    rootNode = await _treeItemService.FillChild(rootNode);
                    treeItems.Add(rootNode);
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    FirstParentNode.Clear();
                    foreach (var item in treeItems)
                    {
                        FirstParentNode.Add(item);
                    }
                });
                return;
            }
        }
        private PilotObjectHelper LastParent { get; set; }

        private ObservableCollection<ICustomTree> _firstParentNode;

        public ObservableCollection<ICustomTree> FirstParentNode
        {

            get => _firstParentNode;
            set
            {
                _firstParentNode = value;
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
            var selected = _dataObjectSelected;
            if (selected == null)
            {
                Info = null;
                return;
            }

            try
            {
                var info = await Task.Run(() => _repoService.GetObjInfo(selected.PilotObjectHelper));

                // Выделение могло смениться, пока грузилась информация
                if (ReferenceEquals(selected, _dataObjectSelected))
                    Info = info;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
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
        private ICommand _selectedValueCommand;
        public ICommand SelectedValueCommand => _selectedValueCommand ??= new RelayCommand<object>(_ => _viewFactory.LookSelection(_dataGridSelected));

        private void CopyToClipboard(string sender)
        {
            var errorText = "Упс, ничего не выбрано.";
            if (_dataObjectSelected == null)
            {
                MessageBox.Show(errorText);
                return;
            }

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
                    Clipboard.SetText(_dataGridSelected?.Description);
                }
                else if (sender == "DataGridSelectLine")
                {
                    Clipboard.SetText(_dataGridSelected?.SenderMemberName + "\t" + _dataGridSelected?.Description);
                }
                else
                {
                    MessageBox.Show(errorText);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private ICommand _copyCommand;
        public ICommand CopyCommand => _copyCommand ??= new RelayCommand<string>(CopyToClipboard);
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
