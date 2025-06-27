using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PilotLookUp.ViewModel
{
    public class AttrVM : INotifyPropertyChanged, IPage
    {
        private PilotObjectHelper _objectHelper;

        public AttrVM(PilotObjectHelper pilotObjectHelper)
        {
            _objectHelper = pilotObjectHelper;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        PagesName IPage.GetName() =>
            PagesName.AttrPage;
    }
}
