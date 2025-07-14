using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class IntHelper : PilotObjectHelper, IPilotObjectHelper
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
            }
            _lookUpObject = value;
            _name = value.ToString();
            _isLookable = false;
        }

        public override BitmapImage GetImage()
        {
            return null;
        }
    }
}
