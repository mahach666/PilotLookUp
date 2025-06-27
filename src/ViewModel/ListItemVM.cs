using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace PilotLookUp.ViewModel
{
    public class ListItemVM : INotifyPropertyChanged, ICustomTree
    {
        public ListItemVM(PilotObjectHelper pilotObjectHelper)
        {
            PilotObjectHelper = pilotObjectHelper;
            Children = new ObservableCollection<ICustomTree>();
        }

        public PilotObjectHelper PilotObjectHelper { get; }
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
