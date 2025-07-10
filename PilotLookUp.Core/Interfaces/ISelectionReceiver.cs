namespace PilotLookUp.Interfaces
{
    using PilotLookUp.Core.Objects;

    /// <summary>
    /// ViewModel, способная принять исходный набор объектов для отображения.
    /// </summary>
    public interface ISelectionReceiver
    {
        void SetInitialSelection(ObjectSet selection);
    }
} 