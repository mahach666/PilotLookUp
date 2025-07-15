using System.Threading.Tasks;

namespace PilotLookUp.Domain.Interfaces
{
    public interface ITreeItemService
    {
        public Task<ICustomTree> FillChild(ICustomTree lastParrent);
    }
}
