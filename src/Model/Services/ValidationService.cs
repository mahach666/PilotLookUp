using PilotLookUp.Domain.Interfaces;
using System;

namespace PilotLookUp.Model.Services
{
    public class ValidationService : IValidationService
    {
        public void ValidateNotNull(object obj, string paramName = null)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName ?? "parameter", "Parameter cannot be null");
        }

        public void ValidateConstructorParams(params object[] args)
        {
            if (args == null) throw new ArgumentNullException("args", "Constructor parameters array is null");
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == null)
                    throw new ArgumentNullException($"Constructor parameter at index {i} is null");
            }
        }

        public void ValidateState(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Object state is null");
        }
    }
} 