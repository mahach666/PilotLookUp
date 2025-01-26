using Ascon.Pilot.SDK;
using PilotLookUp.Commands;
using PilotLookUp.Model;
using PilotLookUp.Objects;
using PilotLookUp.View.UserControls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace PilotLookUp.ViewModel
{
    internal class SearchVM : INotifyPropertyChanged
    {
        private LookUpModel _lookUpModel { get; }

        public SearchVM(LookUpModel lookUpModel)
        {
            _lookUpModel = lookUpModel;
        }

        private List<CastomObjBox> _result;
        public List<CastomObjBox> Result
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
                var res = new List<CastomObjBox>();
                var searchRes = await _lookUpModel.SearchByString(Text);
                {
                    SetRes(searchRes);
                }
            });
        }

        private void SetRes(ObjectSet objectSet)
        {
            var res = new List<CastomObjBox>();
            foreach (var item in objectSet)
            {
                res.Add(new CastomObjBox(item));
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
