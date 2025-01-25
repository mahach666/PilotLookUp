using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.ViewModel;
using System.Windows.Controls;

namespace PilotLookUp.View
{
    /// <summary>
    /// Логика взаимодействия для LookUpView.xaml
    /// </summary>
    public partial class LookUpPage : UserControl, IControl
    {
        LookUpVM _vm { get; set; }
        internal LookUpPage(LookUpVM vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
        }

        PagesName IControl.GetName()
        {
            return PagesName.LookUpPage;
        }
    }
}
