using PilotLookUp.Utils;
using System.Threading;

namespace PilotLookUp.View
{
    public partial class MainView
    {
        private readonly ILogger _logger;
        public MainView(ILogger logger)
        {
            _logger = logger;
            _logger.Trace("[TRACE] MainView: Конструктор вызван");
            // Проверяем, что мы в STA потоке
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                _logger.Trace("[TRACE] MainView: Не STA поток!");
                throw new System.InvalidOperationException(PilotLookUp.Resources.Strings.StaThreadError);
            }
            InitializeComponent();
            _logger.Trace("[TRACE] MainView: InitializeComponent завершён");
        }
    }
}
