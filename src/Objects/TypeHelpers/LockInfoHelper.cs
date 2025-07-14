using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class LockInfoHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public LockInfoHelper(IThemeService themeService, ILockInfo obj)
            : base(themeService)
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