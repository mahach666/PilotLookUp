using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Data;
using PilotLookUp.Objects.TypeHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Lifetime;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.Objects
{
    public class PilotObjectMap
    {
        public PilotObjectMap(IObjectsRepository objectsRepository, PilotObjectHelper senderObj = null, MemberInfo senderMember = null)
        {
            _objectsRepository = objectsRepository;
            _senderObj = senderObj;
            _senderMember = senderMember;
        }
        private IObjectsRepository _objectsRepository { get; }
        private PilotObjectHelper _senderObj { get; }
        private MemberInfo _senderMember { get; }


        public PilotObjectHelper Wrap(object obj)
        {
            return obj switch
            {
                //System
                Enum value => new EnumHelper(value, _objectsRepository),
                string value => new StringHelper(value, _objectsRepository),
                bool value => new BoolHelper(value, _objectsRepository),
                int value => new IntHelper(value, _objectsRepository, _senderObj, _senderMember),
                long value => new LongHelper(value, _objectsRepository),
                DateTime value => new DateTimeHelper(value, _objectsRepository),
                ILease value => new LeaseHelper(value, _objectsRepository),
                Guid value => new GuidHelper(value, _objectsRepository, _senderObj, _senderMember),

                // PilotTypes
                IObjectsRepository value => new ObjectsRepositoryHelper(_objectsRepository),
                IDataObject value => new DataObjectHelper(value, _objectsRepository),
                IHistoryItem value => new HistoryItemHelper(value, _objectsRepository),
                IType type => new TypeHelper(type, _objectsRepository),
                IPerson value => new PersonHelper(value, _objectsRepository),
                IUserState value => new UserStateHelper(value, _objectsRepository),
                IUserStateMachine value => new UserStateMachineHelper(value, _objectsRepository),
                IAttribute value => new AttributeHelper(value, _objectsRepository),
                KeyValuePair<string, object> value => new KeyValuePairHelper(value, _objectsRepository, (IDataObject)_senderObj.LookUpObject),
                KeyValuePair<Guid, int> value => new KeyValuePairHelper(value, _objectsRepository),
                KeyValuePair<IDataObject, int> value => new KeyValuePairHelper(value, _objectsRepository),
                KeyValuePair<Guid, IEnumerable<ITransition>> value => new KeyValuePairHelper(value, _objectsRepository),
                KeyValuePair<int, IAccess> value => new KeyValuePairHelper(value, _objectsRepository),
                IRelation value => new RelationHelper(value, _objectsRepository),
                IFile value => new FileHelper(value, _objectsRepository),
                IAccess value => new AccessHelper(value, _objectsRepository),
                IAccessRecord value => new AccessRecordHelper(value, _objectsRepository),
                IFilesSnapshot value => new FilesSnapshotHelper(value, _objectsRepository),
                IPosition value => new PositionHelper(value, _objectsRepository),
                IOrganisationUnit value => new OrganisationUnitHelper(value, _objectsRepository),
                ITransition value => new TransitionHelper(value, _objectsRepository),
                IStorageDataObject value => new StorageDataObjectHelper(value, _objectsRepository),
                IStateInfo value => new StateInfoHelper(value, _objectsRepository),
                ILockInfo value => new LockInfoHelper(value, _objectsRepository),
                ISignatureRequest value => new SignatureRequestHelper(value),
                ISignature value => new SignatureHelper(value),
                null => new NullHelper(null),

                _ => new OtherHelper(obj, _objectsRepository)
            };
        }

        public static PilotObjectHelper WrapNull()
        {
            return new NullHelper(null);
        }
    }
}
