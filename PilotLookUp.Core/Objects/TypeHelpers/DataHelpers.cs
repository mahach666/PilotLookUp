using Ascon.Pilot.SDK;
using PilotLookUp.Core.Objects;
using System;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Core.Objects.TypeHelpers
{
    public class DataObjectHelper : PilotObjectHelper
    {
        public DataObjectHelper(IDataObject obj)
        {
            _lookUpObject = obj;
            _name = obj.DisplayName;
            _isLookable = true;
            _stringId = obj.Id.ToString();
        }

        public override BitmapImage GetImage()
        {
            // TODO: Inject ISvgToPngConverter dependency
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\dataIcon.png", UriKind.RelativeOrAbsolute));
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

    public class StorageDataObjectHelper : PilotObjectHelper
    {
        public StorageDataObjectHelper(IStorageDataObject obj)
        {
            _lookUpObject = obj;
            _name = obj.DataObject.DisplayName;
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\storageIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
} 
