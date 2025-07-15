namespace PilotLookUp.Domain.Interfaces
{
    public interface IMenuService
    {
        void HandleMenuItemClick(string name);
        void HandleToolbarItemClick(string name);
    }
} 