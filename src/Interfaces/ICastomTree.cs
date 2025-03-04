using PilotLookUp.Objects;
using System.Collections.ObjectModel;

namespace PilotLookUp.Interfaces
{
    public interface ICastomTree
    {
        public ObservableCollection<ICastomTree> Children { get; set; }
        public ICastomTree Parrent { get; set; }
        public PilotObjectHelper PilotObjectHelper { get; }

    }
}
