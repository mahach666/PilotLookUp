using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using System.Windows.Controls;

namespace PilotLookUp.View.UserControls
{
    public partial class SearchPage : UserControl, IControl
    {
        public SearchPage()
        {

            InitializeComponent();
        }

        PagesName IControl.GetName()
        {
            return PagesName.SearchPage;
        }
    }
}
