using Ascon.Pilot.SDK;
using PilotLookUp.Utils;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class DataObjectHelper : PilotObjectHelper
    {
        public DataObjectHelper(IDataObject obj, IObjectsRepository objectsRepository)
        {
            _lookUpObject = obj;
            _name = obj.DisplayName;
            _isLookable = true;
            _stringId = obj.Id.ToString();
            //if (revokedId == null)
            //{
            //    revokedId = objectsRepository.GetUserStates().FirstOrDefault(i => i.Name == "revoked").Id;
            //}
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
