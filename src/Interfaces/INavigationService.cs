namespace PilotLookUp.Interfaces
{
    using PilotLookUp.Enums;
    using PilotLookUp.Objects;

    public interface INavigationService
    {
        void LookSelection(ObjectSet selectedObjects);
        void LookDB();
        void SearchPage();
    }
} 