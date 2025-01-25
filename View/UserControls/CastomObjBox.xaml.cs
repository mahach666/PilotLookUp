using PilotLookUp.Objects;
using System.Windows.Controls;

namespace PilotLookUp.View.UserControls
{
    public partial class CastomObjBox : UserControl
    {
        internal CastomObjBox(PilotObjectHelper vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
