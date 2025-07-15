using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using PilotLookUp.Infrastructure.Converters;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities.Helpers
{
    public class TypeHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public TypeHelper(
            IThemeService themeService,
            IType obj,
            ILogger logger)
            : base(themeService, logger)
        {
            _lookUpObject = obj;
            _name = obj?.Title;
            _isLookable = true;
            _stringId = obj?.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
           return SvgToPngConverter.GetBitmapImageBySvg(((IType)_lookUpObject).SvgIcon);
        }
    }
}
