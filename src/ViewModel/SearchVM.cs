using PilotLookUp.Commands;
using PilotLookUp.Domain.Entities;
using PilotLookUp.Domain.Interfaces;
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
                    var res = await _searchService.SearchAndMapVMsAsync(clipboardText,
                        _navigationService,
                        _tabService,
                        _objectSetFactory,
                        _validationService);
                    if (res?.Count > 0)
                    {
                        Text = clipboardText;
                        Result = res;
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

        public Visibility PromtVisibility => string.IsNullOrEmpty(_text) 
            ? Visibility.Visible 
            : Visibility.Hidden;

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PromtVisibility));
            }
        }
        private void Search()
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                try
                {
                    var res = await _searchService.SearchAndMapVMsAsync(Text, _navigationService, _tabService, _objectSetFactory, _validationService);
                    Result = res;
                }
                catch (System.Exception ex)
                {
                    _errorHandlingService?.HandleError(ex, "SearchVM.Search");
                }
            });
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
