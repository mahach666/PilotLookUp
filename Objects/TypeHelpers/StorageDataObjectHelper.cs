using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class StorageDataObjectHelper : PilotObjectHelper
    {
        public StorageDataObjectHelper(IStorageDataObject obj, IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            LookUpObject = obj;
            Name = obj.DataObject.DisplayName;
        }
    }
}
