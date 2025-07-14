using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects.TypeHelpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace PilotLookUp.ViewModel
{
    public class MainVM : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IErrorHandlingService _errorHandlingService;
        private readonly IValidationService _validationService;

        public MainVM(INavigationService navigationService, IViewModelFactory viewModelFactory, IErrorHandlingService errorHandlingService, IValidationService validationService)
        {
            _validationService = validationService;
            _validationService.ValidateConstructorParams(navigationService, viewModelFactory, errorHandlingService, validationService);
            _navigationService = navigationService;
            _viewModelFactory = viewModelFactory;
            _errorHandlingService = errorHandlingService;
            _navigationService.PageChanged += page => SelectedControl = page;
            
            // Инициализируем SelectedControl только если ActivePage не null
            if (_navigationService.ActivePage != null)
            {
                SelectedControl = _navigationService.ActivePage;
            }
        }

        private IPage _selectedControl;
        public IPage SelectedControl
        {
            get => _selectedControl;
            set
            {
                _selectedControl = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TaskButtVisibilities));
                OnPropertyChanged(nameof(AttrButtVisibilities));
                OnPropertyChanged(nameof(ActivePage));
            }
        }

        public PagesName ActivePage 
        {
            get
            {
                try
                {
                    return _navigationService.ActivePage?.GetName() ?? PagesName.None;
                }
                catch
                {
                    return PagesName.None;
                }
            }
        }

        public Visibility TaskButtVisibilities
        {
            get
            {
                System.Diagnostics.Debug.WriteLine($"[TRACE] TaskButtVisibilities: SelectedControl = {SelectedControl?.GetType().Name}");
                
                if (SelectedControl is LookUpVM lookUpVM)
                {
                    System.Diagnostics.Debug.WriteLine($"[TRACE] TaskButtVisibilities: DataObjectSelected = {lookUpVM.DataObjectSelected?.GetType().Name}");
                    System.Diagnostics.Debug.WriteLine($"[TRACE] TaskButtVisibilities: PilotObjectHelper = {lookUpVM.DataObjectSelected?.PilotObjectHelper?.GetType().Name}");
                    System.Diagnostics.Debug.WriteLine($"[TRACE] TaskButtVisibilities: LookUpObject = {lookUpVM.DataObjectSelected?.PilotObjectHelper?.LookUpObject?.GetType().Name}");
                    System.Diagnostics.Debug.WriteLine($"[TRACE] TaskButtVisibilities: Is IDataObject = {lookUpVM.DataObjectSelected?.PilotObjectHelper?.LookUpObject is IDataObject}");
                    System.Diagnostics.Debug.WriteLine($"[TRACE] TaskButtVisibilities: Is DataObjectHelper = {lookUpVM.DataObjectSelected?.PilotObjectHelper is DataObjectHelper}");
                    
                    if (lookUpVM.DataObjectSelected?.PilotObjectHelper?.LookUpObject is IDataObject || 
                        lookUpVM.DataObjectSelected?.PilotObjectHelper is DataObjectHelper)
                    {
                        System.Diagnostics.Debug.WriteLine($"[TRACE] TaskButtVisibilities: Возвращаем Visible для IDataObject/DataObjectHelper");
                        return Visibility.Visible;
                    }
                }
                else if (SelectedControl is TaskTreeVM || SelectedControl is AttrVM)
                {
                    System.Diagnostics.Debug.WriteLine($"[TRACE] TaskButtVisibilities: Возвращаем Visible для TaskTreeVM/AttrVM");
                    return Visibility.Visible;
                }
                
                System.Diagnostics.Debug.WriteLine($"[TRACE] TaskButtVisibilities: Возвращаем Hidden");
                return Visibility.Hidden;
            }
        }

        public Visibility AttrButtVisibilities => TaskButtVisibilities;

        public ICommand LookDBCommand => new RelayCommand<object>(_ => _navigationService.NavigateToLookUp());
        public ICommand SearchCommand => new RelayCommand<object>(_ => _navigationService.NavigateToSearch());
        public ICommand LookUpPageCommand => new RelayCommand<object>(_ => _navigationService.NavigateTo(PagesName.LookUpPage));
        public ICommand TaskTreeCommand => new RelayCommand<object>(_ => 
        {
            try
            {
                if (SelectedControl is LookUpVM lookUpVM && lookUpVM.DataObjectSelected?.PilotObjectHelper != null)
                {
                    _navigationService.NavigateToTaskTree(lookUpVM.DataObjectSelected.PilotObjectHelper);
                }
            }
            catch (System.Exception ex)
            {
                _errorHandlingService?.HandleError(ex, "MainVM.TaskTreeCommand");
                System.Windows.MessageBox.Show($"Ошибка при переходе к дереву задач: {ex.Message}", 
                    "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        });
        public ICommand AttrCommand => new RelayCommand<object>(_ => 
        {
            try
            {
                if (SelectedControl is LookUpVM lookUpVM && lookUpVM.DataObjectSelected?.PilotObjectHelper != null)
                {
                    _navigationService.NavigateToAttr(lookUpVM.DataObjectSelected.PilotObjectHelper);
                }
            }
            catch (System.Exception ex)
            {
                _errorHandlingService?.HandleError(ex, "MainVM.AttrCommand");
                System.Windows.MessageBox.Show($"Ошибка при переходе к атрибутам: {ex.Message}", 
                    "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        });


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
