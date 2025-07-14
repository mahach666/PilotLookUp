using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Data;
using PilotLookUp.Objects.TypeHelpers;
using System;
using System.Collections.Generic;

namespace PilotLookUp.Objects.Strategies
{
    public class ObjectsRepositoryWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is IObjectsRepository;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new ObjectsRepositoryHelper(context.ObjectsRepository);
    }

    public class DataObjectWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is IDataObject;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new DataObjectHelper((IDataObject)obj);
    }

    public class HistoryItemWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is IHistoryItem;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new HistoryItemHelper((IHistoryItem)obj);
    }

    public class TypeWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is IType;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new TypeHelper((IType)obj);
    }

    public class PersonWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is IPerson;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new PersonHelper((IPerson)obj);
    }

    public class UserStateWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is IUserState;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new UserStateHelper((IUserState)obj);
    }

    public class UserStateMachineWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is IUserStateMachine;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new UserStateMachineHelper((IUserStateMachine)obj);
    }

    public class AttributeWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is IAttribute;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new AttributeHelper((IAttribute)obj);
    }

    public class KeyValuePairStringObjectWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is KeyValuePair<string, object>;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new KeyValuePairHelper((KeyValuePair<string, object>)obj, (IDataObject)context.SenderObj.LookUpObject);
    }

    public class KeyValuePairGuidIntWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is KeyValuePair<Guid, int>;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new KeyValuePairHelper((KeyValuePair<Guid, int>)obj);
    }

    public class KeyValuePairDataObjectIntWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is KeyValuePair<IDataObject, int>;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new KeyValuePairHelper((KeyValuePair<IDataObject, int>)obj);
    }

    public class KeyValuePairGuidEnumerableTransitionWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is KeyValuePair<Guid, IEnumerable<ITransition>>;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new KeyValuePairHelper((KeyValuePair<Guid, IEnumerable<ITransition>>)obj, context.ObjectsRepository);
    }

    public class KeyValuePairIntAccessWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is KeyValuePair<int, IAccess>;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new KeyValuePairHelper((KeyValuePair<int, IAccess>)obj);
    }

    public class RelationWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is IRelation;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new RelationHelper((IRelation)obj);
    }

    public class FileWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is IFile;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new FileHelper((IFile)obj);
    }

    public class AccessWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is IAccess;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new AccessHelper((IAccess)obj);
    }

    public class AccessRecordWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is IAccessRecord;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new AccessRecordHelper((IAccessRecord)obj, context.ObjectsRepository);
    }

    public class FilesSnapshotWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is IFilesSnapshot;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new FilesSnapshotHelper((IFilesSnapshot)obj);
    }

    public class PositionWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is IPosition;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new PositionHelper((IPosition)obj, context.ObjectsRepository);
    }

    public class OrganizationUnitWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is IOrganisationUnit;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new OrganizationUnitHelper((IOrganisationUnit)obj);
    }

    public class TransitionWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is ITransition;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new TransitionHelper((ITransition)obj);
    }

    public class StorageDataObjectWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is IStorageDataObject;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new StorageDataObjectHelper((IStorageDataObject)obj);
    }

    public class StateInfoWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is IStateInfo;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new StateInfoHelper((IStateInfo)obj);
    }

    public class LockInfoWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is ILockInfo;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new LockInfoHelper((ILockInfo)obj);
    }

    public class SignatureRequestWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is ISignatureRequest;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new SignatureRequestHelper((ISignatureRequest)obj);
    }

    public class ReportItemWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is IReportItem;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new ReportItemHelper((IReportItem)obj);
    }

    public class SignatureWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is ISignature;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new SignatureHelper((ISignature)obj);
    }
} 