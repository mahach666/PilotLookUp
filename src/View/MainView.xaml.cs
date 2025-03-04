using PilotLookUp.ViewModel;

namespace PilotLookUp.View
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
