using Ascon.Pilot.SDK;
using PilotLookUp.Utils;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class DataObjectHelper : PilotObjectHelper
    {
        private readonly Guid revokedId = new Guid("abdbe49a-7094-4084-9673-eb5fb3f95262");
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
                    return dataObject.Type.Name.StartsWith("task_");
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
                    && dataObject.Attributes.ContainsKey("state")
                    && dataObject.Attributes["state"].ToString() == revokedId.ToString())
                    return true;
                return false;
            }
        }
    }
}
