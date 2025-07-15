using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using PilotLookUp.Infrastructure.Converters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.Domain.Entities.Helpers
{
    public class IntHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public IntHelper(IThemeService themeService,
            int value,
            IObjectsRepository objectsRepository,
            IPilotObjectHelper sender,
            MemberInfo senderMember,
            ILogger logger)
            : base(themeService, logger)
        {
            if (senderMember != null)
            {
                if (senderMember.Name.Contains("AllOrgUnits"))
                {
                    var unit = objectsRepository.GetOrganisationUnit(value);
                    _lookUpObject = unit;
                    _name = unit?.Title;
                    _isLookable = true;
                    _stringId = unit?.Id.ToString();
                    return;
                }

                else if (sender.LookUpObject is IOrganisationUnit
                                    && senderMember.Name == "Children")
                {
                    var unit = objectsRepository.GetOrganisationUnit(value);
                    _lookUpObject = unit;
                    _name = unit?.Title;
                    _isLookable = true;
                    _stringId = unit?.Id.ToString();
                    return;
                }

                else if (sender.LookUpObject is IOrganisationUnit organisationUnit
                                    && senderMember.Name == "Id")
                {
                    _lookUpObject = organisationUnit;
                    _name = organisationUnit.Title;
                    _isLookable = true;
                    _stringId = organisationUnit.Id.ToString();
                    return;
                }

                else if (sender.LookUpObject is IOrganisationUnit
                    && senderMember.Name == "Person")
                {
                    var person = objectsRepository.GetPerson(value);
                    _lookUpObject = person;
                    _name = person?.DisplayName;
                    _isLookable = true;
                    _stringId = person?.Id.ToString();
                    return;
                }

                else if (sender.LookUpObject is IType type
                    && senderMember.Name == "Id")
                {
                    _lookUpObject = type;
                    _name = type.Title;
                    _isLookable = true;
                    _stringId = type.Id.ToString();
                    return;
                }

                else if (sender.LookUpObject is IType
                    && senderMember.Name == "Children")
                {
                    var typeObj = objectsRepository.GetType(value);
                    _lookUpObject = typeObj;
                    _name = typeObj?.Title;
                    _isLookable = true;
                    _stringId = typeObj?.Id.ToString();
                    return;
                }

                else if (sender.LookUpObject is IPerson person
                    && senderMember.Name == "Id")
                {
                    _lookUpObject = person;
                    _name = person.DisplayName;
                    _isLookable = true;
                    _stringId = person.Id.ToString();
                    return;
                }
                else if (sender.LookUpObject is IPerson
                    && senderMember.Name.Contains("Groups"))
                {
                    var unit = objectsRepository.GetOrganisationUnit(value);
                    _lookUpObject = unit;
                    _name = unit?.Title;
                    _isLookable = true;
                    _stringId = unit?.Id.ToString();
                    return;
                }
                else if (sender.LookUpObject is IDataObject dObj
                    && senderMember.Name == "Subscribers")
                {
                    var personSub = objectsRepository.GetPerson(value);
                    _lookUpObject = personSub;
                    _name = personSub?.DisplayName;
                    _isLookable = true;
                    _stringId = personSub?.Id.ToString();
                    return;
                }

                else if ((sender.LookUpObject is ISignatureRequest || sender.LookUpObject is ISignature)
                    && senderMember.Name.Contains("PositionId"))
                {
                    var unit = objectsRepository.GetOrganisationUnit(value);
                    _lookUpObject = unit;
                    _name = unit?.Title;
                    _isLookable = true;
                    _stringId = unit?.Id.ToString();
                    return;
                }

                else if (sender.LookUpObject is IAccessRecord
                        && (senderMember.Name.Contains("OrgUnitId")
                        || senderMember.Name.Contains("RecordOwner")))
                {
                    var unit = objectsRepository.GetOrganisationUnit(value);
                    _lookUpObject = unit;
                    _name = unit?.Title;
                    _isLookable = true;
                    _stringId = unit?.Id.ToString();
                    return;
                }

                else if (sender.LookUpObject is IStateInfo stateInfo
                        && stateInfo.GetType().Name == "PluginStateInfo"
                        && senderMember.Name == "PersonId")
                {
                    var personObj = objectsRepository.GetPerson(value);
                    _lookUpObject = personObj;
                    _name = personObj?.DisplayName;
                    _isLookable = true;
                    _stringId = personObj?.Id.ToString();
                    return;
                }

                else if (sender.LookUpObject is IStateInfo stateInfo2
                        && stateInfo2.GetType().Name == "PluginStateInfo"
                        && senderMember.Name == "PositionId")
                {
                    var unit = objectsRepository.GetOrganisationUnit(value);
                    _lookUpObject = unit;
                    _name = unit?.Title;
                    _isLookable = true;
                    _stringId = unit?.Id.ToString();
                    return;
                }

                else if (senderMember.Name == "Value"
                    && sender.LookUpObject is KeyValuePair<string,object> valuePair
                    && valuePair.Value is IEnumerable<int>)
                {
                    var unit = objectsRepository.GetOrganisationUnit(value);
                    _lookUpObject = unit;
                    _name = unit?.Title;
                    _isLookable = true;
                    _stringId = unit?.Id.ToString();
                    return;
                }
            }

            else if (sender.LookUpObject is KeyValuePair<IDataObject, int> keyValuePair
                && keyValuePair.Key.Type.Id == value)
            {
                _lookUpObject = keyValuePair.Key.Type;
                _name = keyValuePair.Key.Type.Title;
                _isLookable = true;
                _stringId = keyValuePair.Key.Type.Id.ToString();
                return;
            }

            _lookUpObject = value;
            _name = value.ToString();
            _isLookable = false;
        }

        public override BitmapImage GetImage()
        {
            if (_lookUpObject is IOrganisationUnit)
            {
                return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\organisationUnitIcon.png", UriKind.RelativeOrAbsolute));
            }
            else if (_lookUpObject is IPerson)
            {
                return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\personIcon.png", UriKind.RelativeOrAbsolute));
            }
            else if (_lookUpObject is IType type)
            {
                return SvgToPngConverter.GetBitmapImageBySvg(type.SvgIcon);
            }
            else
            {
                return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\intIcon.png", UriKind.RelativeOrAbsolute));
            }
        }
    }
}
