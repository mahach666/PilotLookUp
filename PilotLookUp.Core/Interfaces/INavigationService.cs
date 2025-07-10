namespace PilotLookUp.Interfaces
{
    using PilotLookUp.Enums;
    using PilotLookUp.Core.Objects;

    public interface INavigationService
    {
        void LookSelection(ObjectSet selectedObjects);
        void LookDB();
        void SearchPage();
    }
} 