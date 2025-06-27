using System.Threading.Tasks;

namespace PilotLookUp.Interfaces
{
    public interface ITreeItemService
    {
        public Task<ICustomTree> FillChild(ICustomTree lastParrent);
    }
}
