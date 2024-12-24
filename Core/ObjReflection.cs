using PilotLookUp.Model.Utils;
using PilotLookUp.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace PilotLookUp.Core
{
    public class ObjReflection
    {
        public ObjReflection(PilotObjectHelper typeHelper)
        {
            object dataObjects = typeHelper?.LookUpObject;

            if (dataObjects != null)
            {
                _objType = dataObjects.GetType();
                _propertyes = _objType.GetProperties();
                _methods = _objType.GetMethods();

                // Поля
                foreach (PropertyInfo property in _propertyes)
                {
                    string name = property.Name;
                    object value = property.GetValue(dataObjects);
                    if (value == null) value = "null";
                    KeyValuePairs.Add(name, value);
                }

                // Методы
                foreach (MethodInfo method in _methods)
                {
                    // Проверяем, что метод не принимает параметры
                    if (method.GetParameters().Length == 0)
                    {
                        string methodName = method.Name + "()";
                        if (methodName.Contains("get_")) continue;
                        object result = method.Invoke(dataObjects, null);
                        if (result == null) result = "null";
                        KeyValuePairs.Add(methodName, result);
                    }
                }
            }
        }

        public static ObjReflection Empty()
        {
            return new ObjReflection(PilotObjectMap.WrapNull());
        }

        private Type _objType { get; }
        private PropertyInfo[] _propertyes { get; }
        private MethodInfo[] _methods { get; }
        public Dictionary<string, object> KeyValuePairs { get; } = new Dictionary<string,object>();
    }
}
