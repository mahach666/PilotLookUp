using PilotLookUp.ViewModel;

namespace PilotLookUp.View
{
    public partial class MainView
    {
        internal MainView(MainVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
