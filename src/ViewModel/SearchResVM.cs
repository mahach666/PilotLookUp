using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.ViewModel
{
    public class SearchResVM
    {
        private readonly IPilotObjectHelper _dataObj;
        private readonly INavigationService _navigationService;
        private readonly ITabService _tabService;
        private readonly IObjectSetFactory _objectSetFactory;
        private readonly IValidationService _validationService;

        public SearchResVM(
             INavigationService navigationService
            , ITabService tabService
            , IPilotObjectHelper pilotObjectHelper
            , IObjectSetFactory objectSetFactory
            , IValidationService validationService)
        {
            _validationService = validationService;
            _validationService.ValidateConstructorParams(navigationService,
                tabService,
                pilotObjectHelper,
                objectSetFactory,
                validationService);
            _dataObj = pilotObjectHelper;
            _navigationService = navigationService;
            _tabService = tabService;
            _objectSetFactory = objectSetFactory;
        }

        public string Name => string.Format(PilotLookUp.Resources.Strings.DisplayNameFormat, _dataObj.Name);
        public string Id => string.Format(PilotLookUp.Resources.Strings.IdFormat, _dataObj.StringId);
        public BitmapImage TypeIcon => _dataObj.GetImage();
        public Visibility CanGo => (_dataObj?.LookUpObject is IDataObject) ? Visibility.Visible : Visibility.Hidden;

        private void GoPage()
        {
            var objectSet = _objectSetFactory.Create(null);
            objectSet.Add(_dataObj);
            _navigationService.NavigateToLookUp(objectSet);
        }

        public ICommand GoPageCommand => new RelayCommand<object>(_ => GoPage());

        private void GoObj()
        {
            if (_dataObj?.LookUpObject is IDataObject dataObj)
                _tabService.GoTo(dataObj);
        }

        public ICommand GoObjCommand => new RelayCommand<object>(_ => GoObj());
    }
}