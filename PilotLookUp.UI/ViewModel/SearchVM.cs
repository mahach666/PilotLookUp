using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Core.Interfaces;
using PilotLookUp.Core.Objects;
using SimpleInjector;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;

namespace PilotLookUp.UI.ViewModel
{
    public class SearchVM : INotifyPropertyChanged, IPage
    {
        private IPageService _pageController { get; }
        private ICustomSearchService _searchService { get; }
        private ITabService _tabService { get; }
        private IClipboardService _clipboard { get; }
        private IDispatcherService _dispatcher { get; }
        private SimpleInjector.Container _container { get; }

        public SearchVM(
            IPageService pageController
            , ICustomSearchService searchService
            , ITabService tabService
            , IClipboardService clipboardService
            , IDispatcherService dispatcherService
            , SimpleInjector.Container container)
        {
            _pageController = pageController;
            _searchService = searchService;
            _tabService = tabService;
            _clipboard = clipboardService;
            _dispatcher = dispatcherService;
            _container = container;
            ClipboardCheck();
        }

        private void ClipboardCheck()
        {
            string clipboardText = _clipboard.GetText();
            _ = _dispatcher.InvokeAsync(async () =>
            {
                var res = new List<UserControl>();
                var searchRes = await _searchService.GetObjByString(clipboardText);
                {
                    if (searchRes?.Count > 0)
                    {
                        Text = clipboardText;
                        SetRes(searchRes);
                    }
                }
            });
        }

        private List<SearchResVM> _result;
        public List<SearchResVM> Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }

        public Visibility PromtVisibility => string.IsNullOrEmpty(_text) ? Visibility.Visible : Visibility.Hidden;

        private string _text;
        public string Text
        {
            get => _text;
            set { _text = value; OnPropertyChanged(); OnPropertyChanged("PromtVisibility"); }
        }
        private void Search()
        {
            _ = _dispatcher.InvokeAsync(async () =>
            {
                var searchRes = await _searchService.GetObjByString(Text);
                SetRes(searchRes);
            });
        }

        private void SetRes(ObjectSet objectSet)
        {
            var res = new List<SearchResVM>();
            var viewModelFactory = _container.GetInstance<IViewModelFactory>();
            foreach (var item in objectSet)
            {
                var vm = viewModelFactory.CreateSearchResVM(item) as SearchResVM;
                res.Add(vm);
            }
            Result = res;
        }

        public ICommand SearchCommand => new RelayCommand<object>(_ => Search());

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        PagesName IPage.GetName() =>
            PagesName.SearchPage;
    }
}
