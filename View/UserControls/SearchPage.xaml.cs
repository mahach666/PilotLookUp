using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.ViewModel;
using System.Windows.Controls;

namespace PilotLookUp.View.UserControls
{
    public partial class SearchPage : UserControl, IControl
    {
        internal SearchPage(SearchVM vm)
        {
            InitializeComponent();
            DataContext = vm;
            asd.Children.Add(new CastomObjBox());
            asd.Children.Add(new CastomObjBox());
        }

        

        PagesName IControl.GetName()
        {
            return PagesName.SearchPage;
        }
    }
}
