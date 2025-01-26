using PilotLookUp.Objects;
using System.Windows.Controls;
using PilotLookUp.ViewModel;

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
