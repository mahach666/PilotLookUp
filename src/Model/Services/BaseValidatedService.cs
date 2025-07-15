using PilotLookUp.Domain.Interfaces;

namespace PilotLookUp.Model.Services
{
    public abstract class BaseValidatedService
    {
        protected BaseValidatedService(IValidationService validationService, params object[] dependencies)
        {
            validationService?.ValidateConstructorParams(dependencies);
        }
    }
} 