using System.Threading.Tasks;

namespace PilotLookUp.Interfaces
{
    public interface ITreeItemService
    {
        public Task<ICastomTree> FillChild(ICastomTree lastParrent);
    }
}
