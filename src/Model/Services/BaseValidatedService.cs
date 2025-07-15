using PilotLookUp.Domain.Interfaces;

namespace PilotLookUp.Model.Services
{
    public abstract class BaseValidatedService
    {
        protected readonly IValidationService _validationService;
        protected BaseValidatedService(IValidationService validationService, params object[] dependencies)
        {
            _validationService = validationService;
            _validationService?.ValidateConstructorParams(dependencies);
        }
    }
} 