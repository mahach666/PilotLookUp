using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Model;
using PilotLookUp.Utils;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PilotLookUp.ViewModel
{
    internal class MainVM : INotifyPropertyChanged
    {
        private LookUpModel _lookUpModel { get; }
        private IPageService _pageController { get; }

        public MainVM(LookUpModel lookUpModel, PagesName startPage = PagesName.None)
        {
            _lookUpModel = lookUpModel;
            _pageController = new PageController(_lookUpModel, startPage, page => SelectedControl = page);
        }

        private IPage _selectedControl;
        public IPage SelectedControl
        {
            get => _selectedControl;
            set
            {
                _selectedControl = value;
                OnPropertyChanged();
            }
        }

        private void GoLookDBPage()
        {
            _pageController.CreatePage(PagesName.DBPage);
        }

        public ICommand LookDBCommand => new RelayCommand<object>(_ => GoLookDBPage());

        private void GoSearchPage()
        {
            _pageController.GoToPage(PagesName.SearchPage);
        }

        public ICommand SearchCommand => new RelayCommand<object>(_ => GoSearchPage());

        public ICommand TaskTreeCommand => new RelayCommand<object>(_ => GoTaskTreePage());

        private void GoTaskTreePage()
        {
            _pageController.CreatePage(PagesName.TaskTree);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
