using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Model;
using PilotLookUp.Objects;
using System.Windows;
using System.Windows.Input;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.ViewModel
{
    internal class SearchResVM
    {
        private LookUpModel _lookUpModel { get; }

        private PilotObjectHelper _dataObj { get; }
        private IPageController _pageController { get; }


        internal SearchResVM(LookUpModel lookUpModel, IPageController pageController, PilotObjectHelper pilotObjectHelper)
        {
            _lookUpModel = lookUpModel;
            _dataObj = pilotObjectHelper;
            _pageController = pageController;
        }

        public string Name => "DisplayName : " + _dataObj.Name;
        public string Id => "Id : " + _dataObj.StringId;
        public Visibility CanGo => (_dataObj?.LookUpObject is IDataObject) ? Visibility.Visible : Visibility.Hidden;

        private void GoPage()
        {
            _pageController.CreatePage(PagesName.LookUpPage, _dataObj);
        }

        public ICommand GoPageCommand => new RelayCommand<object>(_ => GoPage());

        private void GoObj()
        {
            if (_dataObj?.LookUpObject is IDataObject dataObj)
                _lookUpModel.GoTo(dataObj);
        }

        public ICommand GoObjCommand => new RelayCommand<object>(_ => GoObj());
    }
}
