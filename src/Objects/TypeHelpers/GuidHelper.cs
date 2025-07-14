using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using System;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class GuidHelper : PilotObjectHelper, IPilotObjectHelper
    {
        private PilotObjectHelper _sender { get; }
        private MemberInfo _senderMember { get; }
        public GuidHelper(Guid value, PilotObjectHelper sender, MemberInfo senderMember)
        {
            _sender = sender;
            _senderMember = senderMember;

            if (senderMember != null)
            {
                if (senderMember.Name == "Id")
                {
                    _lookUpObject = sender.LookUpObject;
                    _name = sender.Name;
                    _isLookable = true;
                    _stringId = sender.StringId;
                    return;
                }
            }
            _lookUpObject = value;
            _name = value.ToString();
            _isLookable = false;
        }

        public override BitmapImage GetImage()
        {
            if (_senderMember != null)
            {
                if (_senderMember.Name == "Id")
                {
                    return _sender.GetImage();
                }
            }
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\guidIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
