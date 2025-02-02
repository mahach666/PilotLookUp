using PilotLookUp.Objects;
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
        public Brush Color =>  new SolidColorBrush(App.Theme == Ascon.Pilot.Themes.ThemeNames.Jedi ? Colors.Black : Colors.White);
    }
}
