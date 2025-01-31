using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class KeyValuePairHelper : PilotObjectHelper
    {
        // Attr
        public KeyValuePairHelper(KeyValuePair<string, object> keyValuePair, IObjectsRepository objectsRepository, IDataObject sender)
        {
            _lookUpObject = keyValuePair;
            _name = sender.Type.Attributes.FirstOrDefault(i => i.Name == keyValuePair.Key)?.Title ?? keyValuePair.Key;
            _isLookable = true;
        }

        // TypesByChildren
        public KeyValuePairHelper(KeyValuePair<Guid, int> keyValuePair, IObjectsRepository objectsRepository)
        {
            _lookUpObject = keyValuePair;
            _name = keyValuePair.Key.ToString();
            _isLookable = true;
        }
        // TypesByChildren v2
        public KeyValuePairHelper(KeyValuePair<IDataObject, int> keyValuePair, IObjectsRepository objectsRepository)
        {
            _lookUpObject = keyValuePair;
            _name = keyValuePair.Key.DisplayName;
            _isLookable = true;
        }

        // Access
        public KeyValuePairHelper(KeyValuePair<int, IAccess> keyValuePair, IObjectsRepository objectsRepository)
        {
            _lookUpObject = keyValuePair.Value;
            _name = keyValuePair.Value.AccessLevel.ToString();
            _isLookable = true;
        }

        // ITransition
        public KeyValuePairHelper(KeyValuePair<Guid, IEnumerable<ITransition>> keyValuePair, IObjectsRepository objectsRepository)
        {
            _lookUpObject = keyValuePair.Value;
            _name = objectsRepository?.GetUserStates().FirstOrDefault(i => i.Id == keyValuePair.Key)?.Title ?? "invalid";
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            if (_lookUpObject is KeyValuePair<string, object>
                || _lookUpObject is KeyValuePair<Guid, int>
                || _lookUpObject is KeyValuePair<IDataObject, int>)
            {
                return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\keyValuePairIcon.png", UriKind.RelativeOrAbsolute));
            }
            else if (_lookUpObject is IAccess)
            {
                return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\accessIcon.png", UriKind.RelativeOrAbsolute));
            }
            else if (_lookUpObject is IEnumerable<ITransition>)
            {
                return new BitmapImage(new Uri(@"..\..\Resources\TypeIcons\transitionIcon.png", UriKind.RelativeOrAbsolute));
            }
            return null;
        }
    }
}
