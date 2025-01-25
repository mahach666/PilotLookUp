using PilotLookUp.Commands;
using PilotLookUp.Core;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Model;
using PilotLookUp.Objects;
using PilotLookUp.Utils;
using PilotLookUp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PilotLookUp.ViewModel
{
    internal class MainVM : INotifyPropertyChanged
    {
        //internal LookUpView _view;
        private LookUpModel _lookUpModel { get; }
        private PageController _pageController { get; }

        public MainVM(LookUpModel lookUpModel, PagesName startPage)
        {
            _lookUpModel = lookUpModel;
            _pageController = new PageController(_lookUpModel);
            _pageController.GoToPage(startPage);
        }

        public UserControl SelectedControl
        {
            get
            {
                if (_pageController.ActivePage is UserControl userControl)
                    return userControl;
                return null;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
