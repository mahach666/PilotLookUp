using PilotLookUp.Commands;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Enums;
using System.Windows.Input;

namespace PilotLookUp.ViewModel
{
    public class MainVM : BaseValidatedViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IErrorHandlingService _errorHandlingService;
        private readonly IUserNotificationService _notificationService;

        public MainVM(INavigationService navigationService,
            IViewModelFactory viewModelFactory,
            IErrorHandlingService errorHandlingService,
            IValidationService validationService,
            IUserNotificationService notificationService)
            : base(validationService,
                  navigationService,
                  viewModelFactory,
                  errorHandlingService,
                  validationService,
                  notificationService)
        {
            _navigationService = navigationService;
            _viewModelFactory = viewModelFactory;
            _errorHandlingService = errorHandlingService;
            _notificationService = notificationService;
            _navigationService.PageChanged += page => SelectedControl = page;

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

        public ICommand LookDBCommand => new RelayCommand<object>(_ => _navigationService.NavigateToLookUp());
        public ICommand SearchCommand => new RelayCommand<object>(_ => _navigationService.NavigateToSearch());
        public ICommand LookUpPageCommand => new RelayCommand<object>(_ => _navigationService.NavigateTo(PagesName.LookUpPage));
        public ICommand TaskTreeCommand => new Commands.SafeRelayCommand<object>(
            _ =>
            {
                if (SelectedControl is LookUpVM lookUpVM && lookUpVM.DataObjectSelected?.PilotObjectHelper != null)
                {
                    _navigationService.NavigateToTaskTree(lookUpVM.DataObjectSelected.PilotObjectHelper);
                }
            },
            _errorHandlingService,
            _notificationService,
            "MainVM.TaskTreeCommand",
            PilotLookUp.Resources.Strings.ErrorCopyTaskTree
        );
        public ICommand AttrCommand => new Commands.SafeRelayCommand<object>(
            _ =>
            {
                if (SelectedControl is LookUpVM lookUpVM && lookUpVM.DataObjectSelected?.PilotObjectHelper != null)
                {
                    _navigationService.NavigateToAttr(lookUpVM.DataObjectSelected.PilotObjectHelper);
                }
            },
            _errorHandlingService,
            _notificationService,
            "MainVM.AttrCommand",
            PilotLookUp.Resources.Strings.ErrorCopyAttr
        );
    }
}
