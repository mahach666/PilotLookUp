using Ascon.Pilot.SDK;
using PilotLookUp.Objects;
using PilotLookUp.Objects.TypeHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace PilotLookUp.Model
{
    internal class ObjReflection
    {
        internal ObjReflection(PilotObjectHelper typeHelper)
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

        private Type _objType { get; }
        private PropertyInfo[] _propertyes { get; }
        private MethodInfo[] _methods { get; }
        public Dictionary<string, object> KeyValuePairs { get; } = new Dictionary<string, object>();
    }
}
