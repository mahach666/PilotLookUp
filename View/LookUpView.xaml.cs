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
            _vm = vm;
            DataContext = _vm;
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new Page1(_vm));
        }
    }
}
