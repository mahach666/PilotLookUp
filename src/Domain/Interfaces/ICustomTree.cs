using System.Collections.ObjectModel;

namespace PilotLookUp.Domain.Interfaces
{
    public interface ICustomTree
    {
        public ObservableCollection<ICustomTree> Children { get; set; }
        public ICustomTree Parrent { get; set; }
        public IPilotObjectHelper PilotObjectHelper { get; }
    }
}
