using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.ViewModel;
using System.Windows.Controls;

namespace PilotLookUp.View.UserControls
{
    public partial class LookUpPage : UserControl, IControl
    {
        internal LookUpPage(LookUpVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        PagesName IControl.GetName()
        {
            return PagesName.LookUpPage;
        }
    }
}
