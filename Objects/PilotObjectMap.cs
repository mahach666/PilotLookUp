using Ascon.Pilot.SDK;
using PilotLookUp.Extensions;
using PilotLookUp.Model;
using PilotLookUp.Objects.TypeHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.Objects
{
    public class PilotObjectMap
    {
        public PilotObjectMap(IObjectsRepository objectsRepository, PilotObjectHelper senderObj = null)
        {
            _objectsRepository = objectsRepository;
            _senderObj = senderObj;
        }
        private static IObjectsRepository _objectsRepository { get; set; }
        private PilotObjectHelper _senderObj { get; set; }


        public PilotObjectHelper Wrap(object obj)
        {
            return obj switch
            {
                //System
                int value =>  new IntHelper(value, _objectsRepository, _senderObj),
                long value => new LongHelper(value, _objectsRepository),
                string value => new StringHelper(value, _objectsRepository),
                DateTime value => new DateTimeHelper(value,_objectsRepository),
                //bool value ,

                // PilotTypes
                IDataObject value => new DataObjectHelper(value, _objectsRepository),
                IType type => new TypeHelper(type, _objectsRepository),
                IPerson value => new PersonHelper(value, _objectsRepository),
                IUserState value => new UserStateHelper(value, _objectsRepository),
                IUserStateMachine value => new UserStateMachineHelper(value, _objectsRepository),
                IAttribute value => new AttributeHelper(value, _objectsRepository),
                KeyValuePair<string, object> value => new KeyValuePairHelper(value, _objectsRepository,(IDataObject)_senderObj.LookUpObject),
                KeyValuePair<Guid, int> value => new KeyValuePairHelper(value, _objectsRepository),
                KeyValuePair<IDataObject, int> value => new KeyValuePairHelper(value, _objectsRepository),
                KeyValuePair<Guid, IEnumerable<ITransition>> value => new KeyValuePairHelper(value, _objectsRepository),
                IRelation value => new RelationHelper(value, _objectsRepository),
                IFile value => new FileHelper(value, _objectsRepository),
                IAccess value => new AccessHelper(value, _objectsRepository),
                IAccessRecord value => new AccessRecordHelper(value, _objectsRepository),
                IFilesSnapshot value => new FilesSnapshotHelper(value, _objectsRepository),
                IPosition value => new PositionHelper(value, _objectsRepository),
                IOrganisationUnit value => new OrganisationUnitHelper(value, _objectsRepository),
                ITransition value => new TransitionHelper(value, _objectsRepository),
                IStorageDataObject value => new StorageDataObjectHelper(value, _objectsRepository),
                DataState value => new DataStateHelper(value, _objectsRepository),
                null => new NullHelper(null), //new NullHelper(),

                _ => null
            };
        }

        public static PilotObjectHelper WrapNull()
        {
            return new NullHelper(null);
        }
    }
}
