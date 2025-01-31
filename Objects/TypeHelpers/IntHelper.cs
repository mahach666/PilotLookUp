using Ascon.Pilot.SDK;
using PilotLookUp.Extensions;
using PilotLookUp.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class IntHelper : PilotObjectHelper
    {
        private PilotObjectHelper _sender { get; }
        private MemberInfo _senderMember { get; }
        private IObjectsRepository _objectsRepository { get; }
        private int _value { get; }

        public IntHelper(int value, IObjectsRepository objectsRepository, PilotObjectHelper sender, MemberInfo senderMember)
        {
            _sender = sender;
            _senderMember = senderMember;
            _objectsRepository = objectsRepository;
            _value = value;

            if (senderMember != null)
            {
                if (senderMember.Name.Contains("AllOrgUnits"))
                {
                    var unit = objectsRepository.GetOrganisationUnit(value);
                    _lookUpObject = unit;
                    _name = unit?.Title;
                    _isLookable = true;
                    _stringId = unit?.Id.ToString();
                }

                else if (sender.LookUpObject is IOrganisationUnit
                                    && senderMember.Name == "Children")
                {
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
                    _stringId = organisationUnit.Id.ToString(); 
                }

                else if (sender.LookUpObject is IOrganisationUnit
                    && senderMember.Name == "Person")
                {
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
                    var unit = objectsRepository.GetOrganisationUnit(value);
                    _lookUpObject = unit;
                    _name = unit?.Title;
                    _isLookable = true;
                    _stringId = unit?.Id.ToString();
                }
                else if (sender.LookUpObject is IDataObject dObj
                    && senderMember.Name == "Subscribers")
                {
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

        public override BitmapImage GetImage()
        {
            if (_senderMember != null)
            {
                if (_senderMember.Name.Contains("AllOrgUnits"))
                {
                    return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\organisationUnitIcon.png", UriKind.RelativeOrAbsolute));
                }

                else if (_sender.LookUpObject is IOrganisationUnit
                                    && _senderMember.Name == "Children")
                {
                    return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\organisationUnitIcon.png", UriKind.RelativeOrAbsolute));
                }

                else if (_sender.LookUpObject is IOrganisationUnit organisationUnit
                                    && _senderMember.Name == "Id")
                {
                    return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\organisationUnitIcon.png", UriKind.RelativeOrAbsolute));
                }

                else if (_sender.LookUpObject is IOrganisationUnit
                    && _senderMember.Name == "Person")
                {
                    return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\personIcon.png", UriKind.RelativeOrAbsolute));
                }

                else if (_sender.LookUpObject is IType type
                    && _senderMember.Name == "Id")
                {
                    return SvgToPngConverter.GetBitmapImageBySvg(type.SvgIcon);
                }

                else if (_sender.LookUpObject is IType
                    && _senderMember.Name == "Children")
                {
                    var typeObj = _objectsRepository.GetType(_value);
                    return SvgToPngConverter.GetBitmapImageBySvg(typeObj?.SvgIcon);
                }

                else if (_sender.LookUpObject is IPerson person
                    && _senderMember.Name == "Id")
                {
                    return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\personIcon.png", UriKind.RelativeOrAbsolute));
                }
                else if (_sender.LookUpObject is IPerson
                    && _senderMember.Name == "Groups")
                {
                    return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\organisationUnitIcon.png", UriKind.RelativeOrAbsolute));
                }
                else if (_sender.LookUpObject is IDataObject dObj
                    && _senderMember.Name == "Subscribers")
                {
                    return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\personIcon.png", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\intIcon.png", UriKind.RelativeOrAbsolute));
                }
            }

            else if (_sender.LookUpObject is KeyValuePair<IDataObject, int> keyValuePair
                && keyValuePair.Key.Type.Id == _value)
            {
                return SvgToPngConverter.GetBitmapImageBySvg(keyValuePair.Key.Type.SvgIcon);
            }
            else
            {
                return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\intIcon.png", UriKind.RelativeOrAbsolute));
            }
        }
    }
}
