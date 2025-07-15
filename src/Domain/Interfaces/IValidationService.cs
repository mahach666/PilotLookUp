namespace PilotLookUp.Domain.Interfaces
{
    public interface IValidationService
    {
        void ValidateNotNull(object obj, string paramName = null);
        void ValidateConstructorParams(params object[] args);
        void ValidateState(object obj);
    }
} 