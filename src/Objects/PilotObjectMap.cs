using Ascon.Pilot.Bim.SDK;
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
                Enum value => new EnumHelper(value),
                string value => new StringHelper(value),
                bool value => new BoolHelper(value),
                int value => new IntHelper(value, _objectsRepository, _senderObj, _senderMember),
                long value => new LongHelper(value),
                DateTime value => new DateTimeHelper(value),
                ILease value => new LeaseHelper(value),
                Guid value => new GuidHelper(value, _senderObj, _senderMember),

                // PilotTypes
                IObjectsRepository value => new ObjectsRepositoryHelper(_objectsRepository),
                IDataObject value => new DataObjectHelper(value),
                IHistoryItem value => new HistoryItemHelper(value),
                IType type => new TypeHelper(type),
                IPerson value => new PersonHelper(value),
                IUserState value => new UserStateHelper(value),
                IUserStateMachine value => new UserStateMachineHelper(value),
                IAttribute value => new AttributeHelper(value),
                KeyValuePair<string, object> value => new KeyValuePairHelper(value, (IDataObject)_senderObj.LookUpObject),
                KeyValuePair<Guid, int> value => new KeyValuePairHelper(value),
                KeyValuePair<IDataObject, int> value => new KeyValuePairHelper(value),
                KeyValuePair<Guid, IEnumerable<ITransition>> value => new KeyValuePairHelper(value, _objectsRepository),
                KeyValuePair<int, IAccess> value => new KeyValuePairHelper(value),
                IRelation value => new RelationHelper(value),
                IFile value => new FileHelper(value),
                IAccess value => new AccessHelper(value),
                IAccessRecord value => new AccessRecordHelper(value, _objectsRepository),
                IFilesSnapshot value => new FilesSnapshotHelper(value),
                IPosition value => new PositionHelper(value, _objectsRepository),
                IOrganisationUnit value => new OrganizationUnitHelper(value),
                ITransition value => new TransitionHelper(value),
                IStorageDataObject value => new StorageDataObjectHelper(value),
                IStateInfo value => new StateInfoHelper(value),
                ILockInfo value => new LockInfoHelper(value),
                ISignatureRequest value => new SignatureRequestHelper(value),
                IReportItem value => new ReportItemHelper(value),
                ISignature value => new SignatureHelper(value),

                // PilotBimTypes
                IModelElementId value => new ModelElementIdHelper(value),

                null => new NullHelper(),

                _ => new OtherHelper(obj)
            };
        }

        public static PilotObjectHelper WrapNull()
        {
            return new NullHelper();
        }
    }
}
