using PilotLookUp.ViewModel;
using System.Threading;

namespace PilotLookUp.View
{
    public partial class MainView
    {
        public MainView()
        {
            System.Diagnostics.Debug.WriteLine("[TRACE] MainView: Конструктор вызван");
            // Проверяем, что мы в STA потоке
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                System.Diagnostics.Debug.WriteLine("[TRACE] MainView: Не STA поток!");
                throw new System.InvalidOperationException(PilotLookUp.Resources.Strings.StaThreadError);
            }
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("[TRACE] MainView: InitializeComponent завершён");
        }
    }
}
