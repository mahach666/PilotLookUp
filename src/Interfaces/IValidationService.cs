using System;

namespace PilotLookUp.Interfaces
{
    public interface IValidationService
    {
        void ValidateNotNull(object obj, string paramName = null);
        void ValidateConstructorParams(params object[] args);
        void ValidateState(object obj);
        // В будущем: Validate<T>(T obj) для интеграции с FluentValidation
    }
} 