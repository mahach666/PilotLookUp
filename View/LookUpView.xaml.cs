using PilotLookUp.Commands;
using PilotLookUp.VM;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PilotLookUp.View
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class LookUpView : Window
    {
        LookUpVM _vm { get; set; }

        internal LookUpView(LookUpVM vm)
        {
            InitializeComponent();
            vm._view = this;
            _vm = vm;
            DataContext = _vm;
        }
    }
}
