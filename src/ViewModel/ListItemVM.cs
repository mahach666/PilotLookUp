using PilotLookUp.Objects;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace PilotLookUp.ViewModel
{
    public class ListItemVM : INotifyPropertyChanged
    {
        public ListItemVM(PilotObjectHelper pilotObjectHelper)
        {
            PilotObjectHelper = pilotObjectHelper;
            Children = new ObservableCollection<ListItemVM>();
        }

        public PilotObjectHelper PilotObjectHelper { get; }
        public string ObjName => PilotObjectHelper.Name;
        public BitmapImage ObjImage => PilotObjectHelper.GetImage();

        private ObservableCollection<ListItemVM> _children;
        public ObservableCollection<ListItemVM> Children
        {
            get => _children;
            set
            {
                _children = value;
                OnPropertyChanged();
            }
        }

        public ListItemVM Parrent { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
