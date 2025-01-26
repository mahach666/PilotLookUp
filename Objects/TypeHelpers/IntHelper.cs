using Ascon.Pilot.SDK;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class IntHelper : PilotObjectHelper
    {
        public IntHelper(int value, IObjectsRepository objectsRepository, PilotObjectHelper sender, MemberInfo senderMember) : base(objectsRepository)
        {
            if (senderMember != null)
            {
                if (senderMember.Name.Contains("AllOrgUnits"))
                {
                    //var unit = objectsRepository.GetOrganisationUnits().FirstOrDefault(i => i.Id == value);
                    var unit = objectsRepository.GetOrganisationUnit(value);
                    _lookUpObject = unit;
                    _name = unit?.Title;
                    _isLookable = true;
                    _stringId = unit?.Id.ToString();
                }

                else if (sender.LookUpObject is IOrganisationUnit
                                    && senderMember.Name == "Children")
                {
                    //var unit = objectsRepository.GetOrganisationUnits().FirstOrDefault(i => i.Id == value);
                    var unit = objectsRepository.GetOrganisationUnit(value);
                    _lookUpObject = unit;
                    _name = unit?.Title;
                    _isLookable = true;
                    _stringId = unit?.Id.ToString();
                }

                else if (sender.LookUpObject is IOrganisationUnit organisationUnit
                                    && senderMember.Name == "Id")
                {
                    _lookUpObject = organisationUnit;
                    _name = organisationUnit.Title;
                    _isLookable = true;
                    _stringId = organisationUnit.Id.ToString(); ;
                }

                else if (sender.LookUpObject is IOrganisationUnit
                    && senderMember.Name == "Person")
                {
                    //var person = objectsRepository.GetPeople().FirstOrDefault(i => i.Id == value);
                    var person = objectsRepository.GetPerson(value);
                    _lookUpObject = person;
                    _name = person?.DisplayName;
                    _isLookable = true;
                    _stringId = person?.Id.ToString();
                }

                else if (sender.LookUpObject is IType type
                    && senderMember.Name == "Id")
                {
                    _lookUpObject = type;
                    _name = type.Title;
                    _isLookable = true;
                    _stringId = type.Id.ToString();
                }

                else if (sender.LookUpObject is IType
                    && senderMember.Name == "Children")
                {
                    var typeObj = objectsRepository.GetType(value);
                    _lookUpObject = typeObj;
                    _name = typeObj?.Title;
                    _isLookable = true;
                    _stringId = typeObj?.Id.ToString();
                }

                else if (sender.LookUpObject is IPerson person
                    && senderMember.Name == "Id")
                {
                    _lookUpObject = person;
                    _name = person.DisplayName;
                    _isLookable = true;
                    _stringId = person.Id.ToString();
                }
                else if (sender.LookUpObject is IPerson
                    && senderMember.Name == "Groups")
                {
                    //var unit = objectsRepository.GetOrganisationUnits().FirstOrDefault(i => i.Id == value);
                    var unit = objectsRepository.GetOrganisationUnit(value);
                    _lookUpObject = unit;
                    _name = unit?.Title;
                    _isLookable = true;
                    _stringId = unit?.Id.ToString();
                }
                else if (sender.LookUpObject is IDataObject dObj
                    && senderMember.Name == "Subscribers")
                {
                    //var personSub = objectsRepository.GetPeople().FirstOrDefault(i => i.Id == value);
                    var personSub = objectsRepository.GetPerson(value);
                    _lookUpObject = personSub;
                    _name = personSub?.DisplayName;
                    _isLookable = true;
                    _stringId = personSub?.Id.ToString();
                }

                else
                {
                    _lookUpObject = value;
                    _name = value.ToString();
                    _isLookable = false;
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
