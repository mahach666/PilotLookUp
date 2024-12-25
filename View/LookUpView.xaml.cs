using PilotLookUp.ViewModel;

namespace PilotLookUp.View
{
    /// <summary>
    /// Логика взаимодействия для LookUpView.xaml
    /// </summary>
    public partial class LookUpView
    {
        LookUpVM _vm { get; set; }

        internal LookUpView(LookUpVM vm)
        {
            InitializeComponent();
            vm._view = this;
            _vm = vm;
            DataContext = _vm;
        }
    }
}
