using PilotLookUp.Commands;
using PilotLookUp.Interfaces;
using PilotLookUp.Model;
using PilotLookUp.Objects;
using PilotLookUp.View.CastomUIElemens;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PilotLookUp.ViewModel
{
    internal class SearchVM : INotifyPropertyChanged
    {
        private LookUpModel _lookUpModel { get; }
        private IPageController _pageController { get; }

        public SearchVM(LookUpModel lookUpModel, IPageController pageController)
        {
            _lookUpModel = lookUpModel;
            _pageController = pageController;
            ClipboardCheck();
        }

        private void ClipboardCheck()
        {
            string clipboardText = Clipboard.GetText();
            Application.Current.Dispatcher.Invoke(async () =>
            {
                var res = new List<UserControl>();
                var searchRes = await _lookUpModel.SearchByString(clipboardText);
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

        private string _text;
        public string Text
        {
            get => _text;
            set { _text = value; OnPropertyChanged(); }
        }
        private void Search()
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                var searchRes = await _lookUpModel.SearchByString(Text);
                SetRes(searchRes);
            });
        }

        private void SetRes(ObjectSet objectSet)
        {
            var res = new List<SearchResVM>();
            foreach (var item in objectSet)
            {
                var vm = new SearchResVM(_lookUpModel, _pageController, item);
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
    }
}
