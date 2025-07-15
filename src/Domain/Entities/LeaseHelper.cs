using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Utils;
using System.Runtime.Remoting.Lifetime;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities
{
    public class LeaseHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public LeaseHelper(IThemeService themeService,
            ILease obj,
            ILogger logger)
            : base(themeService, logger)
        {
            _lookUpObject = obj;
            _name = obj.ToString();
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return null;
        }
    }
}
