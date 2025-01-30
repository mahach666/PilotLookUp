using PilotLookUp.Objects;
using System.Windows.Media.Imaging;

namespace PilotLookUp.ViewModel
{
    internal class ListItemVM
    {
        public ListItemVM(PilotObjectHelper pilotObjectHelper) => PilotObjectHelper = pilotObjectHelper;
        public PilotObjectHelper PilotObjectHelper { get; }
        public string ObjName => PilotObjectHelper.Name;
        public BitmapImage ObjImage => PilotObjectHelper.GetImage();
    }
}
