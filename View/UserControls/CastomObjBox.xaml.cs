using PilotLookUp.ViewModel;
using System.Windows.Controls;

namespace PilotLookUp.View.UserControls
{
    public partial class CastomObjBox : UserControl
    {
        internal CastomObjBox(CastomObjBoxVM vm)
        {
            InitializeComponent();
            DataContext = vm;           
        }
    }
}
