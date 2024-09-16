using Ascon.Pilot.SDK;
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
        internal ObjReflection(IDataObject dataObjects)
        {
            if (dataObjects != null)
            {
                _objType = dataObjects.GetType();
                _propertyes = _objType.GetProperties();

                foreach (PropertyInfo property in _propertyes)
                {
                    string name = property.Name;
                    object value = property.GetValue(dataObjects);
                    KeyValuePairs.Add(name, value);
                }
            }
        }

        private Type _objType { get; }
        private PropertyInfo[] _propertyes { get; }
        public Dictionary<string, object> KeyValuePairs { get; } = new Dictionary<string, object>();
    }
}
