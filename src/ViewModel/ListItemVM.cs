using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace PilotLookUp.ViewModel
{
    public class ListItemVM : INotifyPropertyChanged, ICastomTree
    {
        public ListItemVM(PilotObjectHelper pilotObjectHelper)
        {
            PilotObjectHelper = pilotObjectHelper;
            Children = new ObservableCollection<ICastomTree>();
        }

        public PilotObjectHelper PilotObjectHelper { get; }
        public string ObjName => PilotObjectHelper.Name;
        public BitmapImage ObjImage => PilotObjectHelper.GetImage();

        private ObservableCollection<ICastomTree> _children;
        public ObservableCollection<ICastomTree> Children
        {
            get => _children;
            set
            {
                _children = value;
                OnPropertyChanged();
            }
        }

        public ICastomTree Parrent { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
