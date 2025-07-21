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
        private PilotObjectHelper _dataObj { get; }
        private IPageService _pageController { get; }
        private ITabService _tabService { get; }


        public SearchResVM(
             IPageService pageController
            , ITabService tabService
            , PilotObjectHelper pilotObjectHelper)
        {
           
            _dataObj = pilotObjectHelper;
            _pageController = pageController;
            _tabService = tabService;
        }

        public string Name => "DisplayName : " + _dataObj.Name;
        public string Id => "Id : " + _dataObj.StringId;
        public BitmapImage TypeIcon => _dataObj.GetImage();
        public Visibility CanGo => (_dataObj?.LookUpObject is IDataObject) ? Visibility.Visible : Visibility.Hidden;

        private void GoPage()
        {
            _pageController.CreatePage(PagesName.LookUpPage, new ObjectSet(null) { _dataObj });
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