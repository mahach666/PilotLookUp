using PilotLookUp.ViewModel;
using System.Threading;

namespace PilotLookUp.View
{
    public partial class MainView
    {
        public MainView()
        {
            // Проверяем, что мы в STA потоке
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                throw new System.InvalidOperationException("MainView должен создаваться в STA потоке");
            }
            
            InitializeComponent();
        }
    }
}
