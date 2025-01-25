using PilotLookUp.Commands;
using PilotLookUp.Core;
using PilotLookUp.Model;
using PilotLookUp.Objects;
using PilotLookUp.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PilotLookUp.ViewModel
{
    internal class MainVM : INotifyPropertyChanged
    {
        //internal LookUpView _view;
        private LookUpModel _lookUpModel;

        public MainVM(LookUpModel lookUpModel)
        {
            _lookUpModel = lookUpModel;
        }

        List<UserControl> _controlsHolder { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
