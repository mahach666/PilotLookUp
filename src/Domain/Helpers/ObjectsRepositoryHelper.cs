using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Helpers
{
    internal class ObjectsRepositoryHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public ObjectsRepositoryHelper(IThemeService themeService,
            IObjectsRepository obj,
            ILogger logger)
            : base(themeService, logger)
        {
            _lookUpObject = obj;
            _name = obj?.GetType().Name;
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\UI\databaseEnabled.png", UriKind.RelativeOrAbsolute));
        }
    }
}
