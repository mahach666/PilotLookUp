using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class IntHelper : PilotObjectHelper
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
                }
            }
            else if (sender.LookUpObject is IPerson person)
            {
                if (person.Id == value)
                {
                    _lookUpObject = person;
                    _name = person.DisplayName;
                    _isLookable = true;
                }
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
