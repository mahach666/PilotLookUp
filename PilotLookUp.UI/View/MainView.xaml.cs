using PilotLookUp.UI.ViewModel;

namespace PilotLookUp.UI.View
{
    public partial class MainView
    {
        public MainView(MainVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
