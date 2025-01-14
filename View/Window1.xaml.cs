using PilotLookUp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PilotLookUp.View
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1
    {
        LookUpVM _vm { get; set; }
        public Window1(LookUpVM vm)
        {
            _vm = vm;
            //DataContext = _vm;
            //InitializeComponent();
        }
    }
}
