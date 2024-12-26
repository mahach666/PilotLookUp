using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class ObjectsRepositoryHelper : PilotObjectHelper
    {
        public ObjectsRepositoryHelper( IObjectsRepository objectsRepository) : base(objectsRepository)
        {
            _lookUpObject = objectsRepository;
            _name = "ObjectsRepository";
            _isLookable = true;
        }
    }
}
