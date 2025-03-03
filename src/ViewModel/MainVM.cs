﻿using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PilotLookUp.ViewModel
{
    internal class MainVM : INotifyPropertyChanged
    {
        private IPageService _pageController { get; }

        public MainVM(IPageService pageService)
        {
            _pageController= pageService;
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
            }
        }

        public ICommand LookDBCommand => new RelayCommand<object>(_ => _pageController.CreatePage(PagesName.DBPage));

        public ICommand SearchCommand => new RelayCommand<object>(_ => _pageController.GoToPage(PagesName.SearchPage));

        public ICommand LookUpPageCommand => new RelayCommand<object>(_ => _pageController.GoToPage(PagesName.LookUpPage));

        public ICommand TaskTreeCommand => new RelayCommand<object>(_ => _pageController.CreatePage(PagesName.TaskTree));


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
