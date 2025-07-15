using PilotLookUp.Domain.Entities;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PilotLookUp.Core
{
    public class ObjReflection
    {
        private readonly ILogger _logger;
        public ObjReflection(IPilotObjectHelper typeHelper, ILogger logger)
        {
            _logger = logger;
            object dataObjects = typeHelper?.LookUpObject;
            _logger.Trace($"[TRACE] ObjReflection: Конструктор вызван для типа {dataObjects?.GetType().FullName}");

            if (dataObjects != null)
            {
                _objType = dataObjects.GetType();
                _propertyes = _objType.GetProperties();
                _methods = _objType.GetMethods();
                _logger.Trace($"[TRACE] ObjReflection: Найдено {_propertyes.Length} свойств, {_methods.Length} методов");

                // Поля
                foreach (PropertyInfo property in _propertyes)
                {
                    _logger.Trace($"[TRACE] ObjReflection: Обрабатывается свойство {property.Name} типа {property.PropertyType.FullName}");
                    object value = property.GetValue(dataObjects);
                    if (value == null) 
                    {
                        value = "null";
                        _logger.Trace($"[TRACE] ObjReflection: Свойство {property.Name} = null");
                    }
                    else
                    {
                        _logger.Trace($"[TRACE] ObjReflection: Свойство {property.Name} = {value} (тип: {value.GetType().FullName})");
                    }
                    KeyValuePairs.Add(property, value);
                }

                // Методы
                foreach (MethodInfo method in _methods)
                {
                    // Проверяем, что метод не принимает параметры
                    if (method.GetParameters().Length == 0)
                    {
                        if (method.Name.Contains("get_")) continue;
                        object result;
                        try
                        {
                             result = method.Invoke(dataObjects, null);
                        }
                        catch (Exception e)
                        {
                            result = "Error: " + e.Message;
                        }
                        if (result == null) result = "null";
                        KeyValuePairs.Add(method, result);
                    }
                }
            }
        }

        public static ObjReflection Empty(ILogger logger)
        {
            return new ObjReflection(PilotObjectMap.WrapNull(), logger);
        }

        private Type _objType { get; }
        private PropertyInfo[] _propertyes { get; }
        private MethodInfo[] _methods { get; }
        public Dictionary<MemberInfo, object> KeyValuePairs { get; } = new Dictionary<MemberInfo, object>();
    }
}
