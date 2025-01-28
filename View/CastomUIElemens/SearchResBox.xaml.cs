using PilotLookUp.ViewModel;
using System.Windows.Controls;

namespace PilotLookUp.View.CastomUIElemens
{
    public partial class SearchResBox : UserControl
    {
        internal SearchResBox(SearchResVM vm)
        {
            InitializeComponent();
            DataContext = vm;           
        }
    }
}
