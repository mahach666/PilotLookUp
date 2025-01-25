using PilotLookUp.ViewModel;

namespace PilotLookUp.View
{
    /// <summary>
    /// Логика взаимодействия для LookUpView.xaml
    /// </summary>
    public partial class LookUpView
    {
        //private MainVM _vm { get; set; }

        internal LookUpView(MainVM vm)
        {
            InitializeComponent();
            //_vm = vm;
            DataContext = vm;
        }
    }
}
