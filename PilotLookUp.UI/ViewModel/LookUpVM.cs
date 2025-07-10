using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Core.Objects;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Application = System.Windows.Application;

namespace PilotLookUp.UI.ViewModel
{
    public class LookUpVM : INotifyPropertyChanged, IPage, ISelectionReceiver
    {
        private IRepoService _repoService;
        private IWindowService _windowService;
        private readonly IClipboardService _clipboard;
        private readonly IDispatcherService _dispatcher;

        public LookUpVM(IRepoService lookUpModel, 
            IWindowService windowService,
            IClipboardService clipboardService,
            IDispatcherService dispatcherService)
        {
            _repoService = lookUpModel;
            _windowService = windowService;
            _clipboard = clipboardService;
            _dispatcher = dispatcherService;
            // Загружаем объекты репозитория по умолчанию
            LoadDefaultSelection();
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
                OnPropertyChanged();
                UpdateFiltredDataObjectsAsync();
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
                OnPropertyChanged("PromtVisibility");
                UpdateFiltredDataObjectsAsync();
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
            if (SearchText?.Length >= 2)
            {
                var filtered = await Task.Run(() =>
                    SelectionDataObjects
                        .Where(i => i.ObjName.ToUpper().Contains(SearchText.ToUpper())
                                 || i.StrId.ToUpper().Contains(SearchText.ToUpper()))
                        .ToList()
                );

                _dispatcher.Invoke(() => FiltredDataObjects = filtered);
            }
            else
            {
                _dispatcher.Invoke(() => FiltredDataObjects = SelectionDataObjects);
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
                Info = await _repoService.GetObjInfo(_dataObjectSelected.PilotObjectHelper);
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
                    _clipboard.SetText(_dataObjectSelected.PilotObjectHelper?.Name);
                }
                else if (sender == "DataGridSelectName")
                {
                    _clipboard.SetText(_dataGridSelected?.SenderMemberName);
                }
                else if (sender == "DataGridSelectValue")
                {
                    _clipboard.SetText(_dataGridSelected?.Discription);
                }
                else if (sender == "DataGridSelectLine")
                {
                    _clipboard.SetText(_dataGridSelected?.SenderMemberName + "\t" + _dataGridSelected?.Discription);
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
        public ICommand SelectedValueCommand => new RelayCommand<ObjectSet>(obj =>
        {
            if (obj != null)
            {
                _windowService.CreateNewMainWindow(obj);
            }
        });


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        PagesName IPage.GetName() =>
             PagesName.LookUpPage;

        private void LoadDefaultSelection()
        {
            Task.Run(() =>
            {
                var objectSet = _repoService.GetWrapedRepo();
                if (objectSet != null && objectSet.Any())
                {
                    var list = objectSet.Select(o => new ListItemVM(o)).ToList();
                    _dispatcher.Invoke(() => SelectionDataObjects = list);
                }
            });
        }

        // Метод вызывается PageService для установки стартового набора выбранных объектов
        public void SetInitialSelection(ObjectSet selection)
        {
            if (selection == null || !selection.Any()) return;

            Task.Run(() =>
            {
                var list = selection.Select(o => new ListItemVM(o)).ToList();
                _dispatcher.Invoke(() => SelectionDataObjects = list);
            });
        }
    }
}
