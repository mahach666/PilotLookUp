using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class KeyValuePairHelper : PilotObjectHelper
    {
        // Attr
        public KeyValuePairHelper(KeyValuePair<string, object> keyValuePair, IDataObject sender)
        {
            LookUpObject = keyValuePair;
            Name = sender.Type.Attributes.FirstOrDefault(i=>i.Name == keyValuePair.Key)?.Title ?? keyValuePair.Key;
            //Name = keyValuePair.Key;
        }

        // TypesByChildren
        public KeyValuePairHelper(KeyValuePair<Guid, int> keyValuePair)
        {
            LookUpObject = keyValuePair.Value;
            Name = keyValuePair.Key.ToString(); ;
        }

        //// Access
        //public KeyValuePairHelper(KeyValuePair<int, IAccess> keyValuePair)
        //{
        //    LookUpObject = keyValuePair.Value;
        //    Name = keyValuePair.Value.AccessLevel.ToString(); ;
        //}

        // ITransition
        public KeyValuePairHelper(KeyValuePair<Guid, IEnumerable<ITransition>> keyValuePair, IObjectsRepository objectsRepository)
        {
            LookUpObject = keyValuePair.Value;
            Name = objectsRepository?.GetUserStates().FirstOrDefault(i => i.Id == keyValuePair.Key)?.Title ?? "invalid"; ;
        }
    }
}
