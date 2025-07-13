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

        public LookUpVM(IRepoService lookUpModel, 
            IWindowService windowService)
        {
            _repoService = lookUpModel;
            _windowService = windowService;
        }

        private void LoadDataFromRepository()
        {
            try
            {
                var repoData = _repoService.GetWrapedRepo();
                if (repoData != null && repoData.Any())
                {
                    var listItems = repoData.Select(x => new ListItemVM(x)).ToList();
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
                if (value == null || !value.Any()) 
                {
                    // Если передали пустые данные, загружаем из репозитория
                    LoadDataFromRepository();
                    return;
                }
                _selectionDataObjects = value;
                _dataInitialized = true; // Отмечаем, что данные были установлены через свойство
                DataObjectSelected = value?.FirstOrDefault();
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
                var filtered = await Task.Run(() =>
                    SelectionDataObjects
                        .Where(i => i.ObjName.ToUpper().Contains(SearchText.ToUpper())
                                 || i.StrId.ToUpper().Contains(SearchText.ToUpper()))
                        .ToList()
                );

                Application.Current.Dispatcher.Invoke(() => FiltredDataObjects = filtered);
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() => FiltredDataObjects = SelectionDataObjects);
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
