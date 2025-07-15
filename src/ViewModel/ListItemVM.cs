using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace PilotLookUp.ViewModel
{
    public class ListItemVM : BaseValidatedViewModel, ICustomTree
    {
        private readonly ILogger _logger;

        public ListItemVM(
            IPilotObjectHelper pilotObjectHelper,
            IValidationService validationService,
            ILogger logger)
            : base(validationService, pilotObjectHelper, logger)
        {
            _logger = logger;
            _logger.Trace($"[TRACE] ListItemVM: Конструктор вызван для {pilotObjectHelper?.Name ?? "null"}");
            PilotObjectHelper = pilotObjectHelper;
            Children = new ObservableCollection<ICustomTree>();
        }

        public IPilotObjectHelper PilotObjectHelper { get; }
        public string ObjName => PilotObjectHelper.Name;
        public string StrId => PilotObjectHelper.StringId;
        public BitmapImage ObjImage => PilotObjectHelper.GetImage();

        private ObservableCollection<ICustomTree> _children;
        public ObservableCollection<ICustomTree> Children
        {
            get => _children;
            set
            {
                _children = value;
                OnPropertyChanged();
            }
        }

        public ICustomTree Parrent { get; set; }
    }
}
