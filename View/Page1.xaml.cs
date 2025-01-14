using PilotLookUp.ViewModel;
using System.Windows.Controls;

namespace PilotLookUp.View
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 
    {
        LookUpVM _vm { get; set; }

        internal Page1(LookUpVM vm)
        {
            _vm = vm;
            DataContext = _vm;
            InitializeComponent();
        }
    }
}
