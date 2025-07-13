using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PilotLookUp.ViewModel
{
    public class SearchVM : INotifyPropertyChanged, IPage
    {
        private readonly INavigationService _navigationService;
        private readonly ICustomSearchService _searchService;
        private readonly ITabService _tabService;
        private readonly IObjectSetFactory _objectSetFactory;

        public SearchVM(
            INavigationService navigationService
            , ICustomSearchService searchService
            , ITabService tabService
            , IObjectSetFactory objectSetFactory)
        {
            _navigationService = navigationService;
            _searchService = searchService;
            _tabService = tabService;
            _objectSetFactory = objectSetFactory;
            ClipboardCheck();
        }

        private void ClipboardCheck()
        {
            string clipboardText = Clipboard.GetText();
            Application.Current.Dispatcher.Invoke(async () =>
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
            Application.Current.Dispatcher.Invoke(async () =>
            {
                var searchRes = await _searchService.GetObjByString(Text);
                SetRes(searchRes);
            });
        }

        private void SetRes(ObjectSet objectSet)
        {
            var res = new List<SearchResVM>();
            foreach (var item in objectSet)
            {
                var vm = new SearchResVM(_navigationService, _tabService, item, _objectSetFactory);
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
