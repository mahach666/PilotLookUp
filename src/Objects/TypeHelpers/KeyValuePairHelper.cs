using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using PilotLookUp.Interfaces;
using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class KeyValuePairHelper : PilotObjectHelper, IPilotObjectHelper
    {
        // Attr
        public KeyValuePairHelper(KeyValuePair<string, object> keyValuePair, IDataObject sender)
        {
            _lookUpObject = keyValuePair;
            _name = sender.Type.Attributes.FirstOrDefault(i => i.Name == keyValuePair.Key)?.Title ?? keyValuePair.Key;
            _isLookable = true;
        }

        // TypesByChildren
        public KeyValuePairHelper(KeyValuePair<Guid, int> keyValuePair)
        {
            _lookUpObject = keyValuePair;
            _name = keyValuePair.Key.ToString();
            _isLookable = true;
        }
        // TypesByChildren v2
        public KeyValuePairHelper(KeyValuePair<IDataObject, int> keyValuePair)
        {
            _lookUpObject = keyValuePair;
            _name = keyValuePair.Key.DisplayName;
            _isLookable = true;
        }

        // Access
        public KeyValuePairHelper(KeyValuePair<int, IAccess> keyValuePair)
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
            return null;
        }
    }
}
