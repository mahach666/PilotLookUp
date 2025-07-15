using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Helpers
{
    internal class GuidHelper : PilotObjectHelper, IPilotObjectHelper
    {
        private IPilotObjectHelper _sender { get; }
        private MemberInfo _senderMember { get; }
        internal GuidHelper(IThemeService themeService,
            Guid value,
            ILogger logger)
            : base(themeService, logger)
        {
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
