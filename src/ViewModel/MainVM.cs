using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects.TypeHelpers;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace PilotLookUp.ViewModel
{
    public class MainVM : INotifyPropertyChanged, IDisposable
    {
        private IPageService _pageController { get; }

        public MainVM(IPageService pageService)
        {
            _pageController = pageService;
            _pageController.PageChanged += page => SelectedControl = page;
            SelectedControl = _pageController.ActivePage;
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

        public PagesName ActivePage => _pageController.ActivePage.GetName();

        private bool _isTopmost;
        public bool IsTopmost
        {
            get => _isTopmost;
            set
            {
                _isTopmost = value;
                OnPropertyChanged();
            }
        }

        public Visibility TaskButtVisibilities
        {
            get
            {
                if (SelectedControl is LookUpVM lookUpVM
                        && lookUpVM.DataObjectSelected is ICustomTree customTree
                        && customTree.PilotObjectHelper is DataObjectHelper)
                    return Visibility.Visible;
                else if (SelectedControl is TaskTreeVM || SelectedControl is AttrVM)
                    return Visibility.Visible;
                return Visibility.Hidden;
            }
        }

        public Visibility AttrButtVisibilities => TaskButtVisibilities;

        private ICommand _lookDBCommand;
        public ICommand LookDBCommand => _lookDBCommand ??= new RelayCommand<object>(_ => _pageController.CreatePage(PagesName.DBPage));

        private ICommand _searchCommand;
        public ICommand SearchCommand => _searchCommand ??= new RelayCommand<object>(_ => _pageController.GoToPage(PagesName.SearchPage));

        private ICommand _lookUpPageCommand;
        public ICommand LookUpPageCommand => _lookUpPageCommand ??= new RelayCommand<object>(_ => _pageController.GoToPage(PagesName.LookUpPage));

        private ICommand _taskTreeCommand;
        public ICommand TaskTreeCommand => _taskTreeCommand ??= new RelayCommand<object>(_ => _pageController.CreatePage(PagesName.TaskTree));

        private ICommand _attrCommand;
        public ICommand AttrCommand => _attrCommand ??= new RelayCommand<object>(_ => _pageController.CreatePage(PagesName.AttrPage));

        private ICommand _eventMonitorCommand;
        public ICommand EventMonitorCommand => _eventMonitorCommand ??= new RelayCommand<object>(_ => _pageController.GoToPage(PagesName.EventMonitor));

        public void Dispose() => (_pageController as IDisposable)?.Dispose();


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
