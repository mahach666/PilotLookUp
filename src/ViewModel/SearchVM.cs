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
        private readonly IErrorHandlingService _errorHandlingService;
        private readonly IValidationService _validationService;
        private readonly IClipboardService _clipboardService;

        public SearchVM(
            INavigationService navigationService
            , ICustomSearchService searchService
            , ITabService tabService
            , IObjectSetFactory objectSetFactory
            , IErrorHandlingService errorHandlingService
            , IValidationService validationService
            , IClipboardService clipboardService)
        {
            _validationService = validationService;
            _validationService.ValidateConstructorParams(navigationService,
                searchService,
                tabService,
                objectSetFactory,
                errorHandlingService,
                validationService,
                clipboardService);
            _navigationService = navigationService;
            _searchService = searchService;
            _tabService = tabService;
            _objectSetFactory = objectSetFactory;
            _errorHandlingService = errorHandlingService;
            _clipboardService = clipboardService;
            ClipboardCheck();
        }

        private void ClipboardCheck()
        {
            string clipboardText = _clipboardService.GetClipboardText();
            Application.Current.Dispatcher.Invoke(async () =>
            {
                try
                {
                    var res = new List<UserControl>();
                    var searchRes = await _searchService.GetObjByString(clipboardText);
                    if (searchRes?.Count > 0)
                    {
                        Text = clipboardText;
                        SetRes(searchRes);
                    }
                }
                catch (System.Exception ex)
                {
                    _errorHandlingService?.HandleError(ex, "SearchVM.ClipboardCheck");
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
                try
                {
                    var searchRes = await _searchService.GetObjByString(Text);
                    SetRes(searchRes);
                }
                catch (System.Exception ex)
                {
                    _errorHandlingService?.HandleError(ex, "SearchVM.Search");
                }
            });
        }

        private void SetRes(ObjectSet objectSet)
        {
            var res = new List<SearchResVM>();
            foreach (var item in objectSet)
            {
                var vm = new SearchResVM(_navigationService, _tabService, item, _objectSetFactory, _validationService);
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
