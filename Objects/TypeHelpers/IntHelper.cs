using Ascon.Pilot.SDK;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class IntHelper : PilotObjectHelper
    {
        public IntHelper(int value, IObjectsRepository objectsRepository, PilotObjectHelper sender) : base(objectsRepository)
        {
            if (sender.LookUpObject is IType type)
            {
                if (type.Id == value)
                {
                    _lookUpObject = type;
                    _name = type.Title;
                    _isLookable = true;
                    _stringId = type.Id.ToString();
                }
            }
            else if (sender.LookUpObject is IPerson person)
            {
                if (person.Id == value)
                {
                    _lookUpObject = person;
                    _name = person.DisplayName;
                    _isLookable = true;
                    _stringId = person.Id.ToString();
                }
            }
            else if (sender.LookUpObject is KeyValuePair<IDataObject, int> keyValuePair
                && keyValuePair.Key.Type.Id == value)
            {
                _lookUpObject = keyValuePair.Key.Type;
                _name = keyValuePair.Key.Type.Title;
                _isLookable = true;
                _stringId = keyValuePair.Key.Type.Id.ToString();
            }
            else
            {
                _lookUpObject = value;
                _name = value.ToString();
                _isLookable = false;
            }
        }
    }
}
