using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Utils;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Entities
{
    public class DataObjectHelper : PilotObjectHelper, IPilotObjectHelper
    {
        public DataObjectHelper(IThemeService themeService, IDataObject obj)
            : base(themeService)
        {
            _lookUpObject = obj;
            _name = obj?.DisplayName;
            _isLookable = true;
            _stringId = obj?.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            return SvgToPngConverter.GetBitmapImageBySvg(((IDataObject)_lookUpObject).Type.SvgIcon);
        }

        public bool IsTask
        {
            get
            {
                if (_lookUpObject != null && _lookUpObject is IDataObject dataObject)
                {
                    return dataObject.Type.Name.StartsWith(SystemTypeNames.TASK_PREFIX);
                }
                return false;
            }
        }

        public bool IsRevokedTask
        {
            get
            {
                if (IsTask
                    && _lookUpObject is IDataObject dataObject
                    && dataObject.Attributes.TryGetValue("state", out var res)
                    && res.ToString() == SystemStates.TASK_REVOKED_STATE_ID.ToString())
                    return true;
                return false;
            }
        }
    }
}
