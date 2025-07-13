using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.ViewModel
{
    public class SearchResVM
    {
        private readonly PilotObjectHelper _dataObj;
        private readonly INavigationService _navigationService;
        private readonly ITabService _tabService;

        public SearchResVM(
             INavigationService navigationService
            , ITabService tabService
            , PilotObjectHelper pilotObjectHelper)
        {
           
            _dataObj = pilotObjectHelper;
            _navigationService = navigationService;
            _tabService = tabService;
        }

        public string Name => "DisplayName : " + _dataObj.Name;
        public string Id => "Id : " + _dataObj.StringId;
        public BitmapImage TypeIcon => _dataObj.GetImage();
        public Visibility CanGo => (_dataObj?.LookUpObject is IDataObject) ? Visibility.Visible : Visibility.Hidden;

        private void GoPage()
        {
            _navigationService.NavigateToLookUp(new ObjectSet(null) { _dataObj });
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