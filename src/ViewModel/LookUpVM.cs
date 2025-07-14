using PilotLookUp.Commands;
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
using Application = System.Windows.Application;

namespace PilotLookUp.ViewModel
{
    public class LookUpVM : INotifyPropertyChanged, IPage
    {
        private IRepoService _repoService;
        private IWindowService _windowService;
        private bool _dataInitialized = false;
        private readonly IErrorHandlingService _errorHandlingService;
        private readonly IValidationService _validationService;

        public LookUpVM(IRepoService lookUpModel, IWindowService windowService, IErrorHandlingService errorHandlingService, IValidationService validationService)
        {
            _validationService = validationService;
            _validationService.ValidateConstructorParams(lookUpModel, windowService, errorHandlingService, validationService);
            System.Diagnostics.Debug.WriteLine("[TRACE] LookUpVM: Конструктор вызван");
            _repoService = lookUpModel;
            _windowService = windowService;
            _errorHandlingService = errorHandlingService;
        }

        private void LoadDataFromRepository()
        {
            try
            {
                var repoData = _repoService.GetWrapedRepo();
                if (repoData != null && repoData.Any())
                {
                    var listItems = repoData.Where(x => x != null).Select(x => {
                        if (x == null) {
                            System.Diagnostics.Debug.WriteLine("[LookUpVM] Обнаружен null в repoData");
                            return null;
                        }
                        return new ListItemVM(x, _validationService);
                    }).Where(x => x != null).ToList();
                    _selectionDataObjects = listItems;
                    OnPropertyChanged(nameof(SelectionDataObjects));
                    
                    // Устанавливаем первый элемент как выбранный
                    DataObjectSelected = listItems.FirstOrDefault();
                    
                    _ = UpdateFiltredDataObjectsAsync();
                }
                else
                {
                    System.Windows.MessageBox.Show("Репозиторий пуст или недоступен.", "Информация", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
            }
            catch (System.Exception ex)
            {
                _errorHandlingService?.HandleError(ex, "LookUpVM.LoadDataFromRepository");
                System.Windows.MessageBox.Show($"Ошибка при загрузке данных из репозитория: {ex.Message}\n\n{ex.StackTrace}", 
                    "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private List<ListItemVM> _selectionDataObjects;
        public List<ListItemVM> SelectionDataObjects
        {
            get => _selectionDataObjects;
            set
            {
                System.Diagnostics.Debug.WriteLine($"[TRACE] LookUpVM.SelectionDataObjects.set: value is null? {value == null}, count: {value?.Count ?? 0}");
                if (value != null)
                {
                    foreach (var item in value)
                    {
                        System.Diagnostics.Debug.WriteLine($"[TRACE] LookUpVM.SelectionDataObjects.set: ListItemVM.ObjName: {item?.ObjName}");
                    }
                }
                if (value == null || !value.Any()) 
                {
                    // Если передали пустые данные, загружаем из репозитория
                    LoadDataFromRepository();
                    return;
                }
                var filtered = value.Where(x => x != null).ToList();
                if (filtered.Count != value.Count)
                {
                    System.Diagnostics.Debug.WriteLine($"[LookUpVM] Обнаружены null в SelectionDataObjects: {value.Count - filtered.Count}");
                }
                _selectionDataObjects = filtered;
                _dataInitialized = true; // Отмечаем, что данные были установлены через свойство
                DataObjectSelected = filtered.FirstOrDefault();
                OnPropertyChanged();
                _ = UpdateFiltredDataObjectsAsync();
            }
        }

        // Метод для инициализации данных, если они не были установлены через свойство
        public void InitializeDataIfNeeded()
        {
            if (!_dataInitialized)
            {
                LoadDataFromRepository();
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
            if (SearchText?.Length >= 2)
            {
                var filtered = SelectionDataObjects
                    .Where(i => i.ObjName.ToUpper().Contains(SearchText.ToUpper())
                             || i.StrId.ToUpper().Contains(SearchText.ToUpper()))
                    .ToList();
                await Application.Current.Dispatcher.InvokeAsync(() => FiltredDataObjects = filtered);
            }
            else
            {
                await Application.Current.Dispatcher.InvokeAsync(() => FiltredDataObjects = SelectionDataObjects);
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
            catch (System.Exception ex)
            {
                _errorHandlingService?.HandleError(ex, "LookUpVM.CopyToClipboard");
                MessageBox.Show(errorText);
            }
        }

        public ICommand CopyCommand => new RelayCommand<string>(CopyToClipboard);
        public ICommand SelectedValueCommand => new RelayCommand<object>(_ => _windowService.CreateNewMainWindow(_dataGridSelected));


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        PagesName IPage.GetName() =>
             PagesName.LookUpPage;
    }
}
