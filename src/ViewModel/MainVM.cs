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
        private readonly IUserNotificationService _notificationService;

        public MainVM(INavigationService navigationService, IViewModelFactory viewModelFactory, IErrorHandlingService errorHandlingService, IValidationService validationService, IUserNotificationService notificationService)
        {
            _validationService = validationService;
            _validationService.ValidateConstructorParams(navigationService, viewModelFactory, errorHandlingService, validationService, notificationService);
            _navigationService = navigationService;
            _viewModelFactory = viewModelFactory;
            _errorHandlingService = errorHandlingService;
            _notificationService = notificationService;
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
                if (SelectedControl is LookUpVM lookUpVM)
                {
                    if (lookUpVM.DataObjectSelected?.PilotObjectHelper?.LookUpObject is IDataObject || 
                        lookUpVM.DataObjectSelected?.PilotObjectHelper is DataObjectHelper)
                    {
                        return Visibility.Visible;
                    }
                }
                else if (SelectedControl is TaskTreeVM || SelectedControl is AttrVM)
                {
                    return Visibility.Visible;
                }
                
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
                _notificationService.ShowError($"Ошибка при переходе к дереву задач: {ex.Message}");
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
                _notificationService.ShowError($"Ошибка при переходе к атрибутам: {ex.Message}");
            }
        });


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
