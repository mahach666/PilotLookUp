using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Helpers
{
    public class LockInfoHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public LockInfoHelper(IThemeService themeService,
            ILockInfo obj,
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