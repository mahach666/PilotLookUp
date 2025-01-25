using PilotLookUp.Enums;
using PilotLookUp.Model;
using PilotLookUp.Utils;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace PilotLookUp.ViewModel
{
    internal class MainVM : INotifyPropertyChanged
    {
        private LookUpModel _lookUpModel { get; }
        private PageController _pageController { get; }

        public MainVM(LookUpModel lookUpModel, PagesName startPage)
        {
            _lookUpModel = lookUpModel;
            _pageController = new PageController(_lookUpModel);
            _pageController.GoToPage(startPage);
        }

        public UserControl SelectedControl
        {
            get
            {
                if (_pageController.ActivePage is UserControl userControl)
                    return userControl;
                return null;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
