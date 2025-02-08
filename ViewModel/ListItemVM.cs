using PilotLookUp.Objects;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brush = System.Windows.Media.Brush;

namespace PilotLookUp.ViewModel
{
    internal class ListItemVM
    {
        public ListItemVM(PilotObjectHelper pilotObjectHelper) => PilotObjectHelper = pilotObjectHelper;
        public PilotObjectHelper PilotObjectHelper { get; }
        public string ObjName => PilotObjectHelper.Name;
        public BitmapImage ObjImage => PilotObjectHelper.GetImage();

        public ListItemVM Parrent;
        public ObservableCollection<ListItemVM> Children;
    }
}
