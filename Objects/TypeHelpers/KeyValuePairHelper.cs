using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class KeyValuePairHelper : PilotObjectHelper
    {
        // Attr
        public KeyValuePairHelper(KeyValuePair<string, object> keyValuePair, IObjectsRepository objectsRepository, IDataObject sender) : base(objectsRepository)
        {
            _lookUpObject = keyValuePair;
            _name = sender.Type.Attributes.FirstOrDefault(i=>i.Name == keyValuePair.Key)?.Title ?? keyValuePair.Key;
            _isLookable = true;
        }

        // TypesByChildren
        public KeyValuePairHelper(KeyValuePair<Guid, int> keyValuePair, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = keyValuePair;
            _name = keyValuePair.Key.ToString();
            _isLookable = true;
        }
        // TypesByChildren v2
        public KeyValuePairHelper(KeyValuePair<IDataObject, int> keyValuePair, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = keyValuePair;
            _name = keyValuePair.Key.DisplayName;
            _isLookable = true;
        }

        //// Access
        //public KeyValuePairHelper(KeyValuePair<int, IAccess> keyValuePair)
        //{
        //    LookUpObject = keyValuePair.Value;
        //    Name = keyValuePair.Value.AccessLevel.ToString(); ;
        //}

        // ITransition
        public KeyValuePairHelper(KeyValuePair<Guid, IEnumerable<ITransition>> keyValuePair, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = keyValuePair.Value;
            _name = objectsRepository?.GetUserStates().FirstOrDefault(i => i.Id == keyValuePair.Key)?.Title ?? "invalid";
            _isLookable = true;
        }
    }
}
