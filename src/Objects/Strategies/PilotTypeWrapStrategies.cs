using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Data;
using PilotLookUp.Objects.TypeHelpers;
using System;
using System.Collections.Generic;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Objects.Strategies
{
    public class ObjectsRepositoryWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public ObjectsRepositoryWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is IObjectsRepository;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateObjectsRepository((IObjectsRepository)obj);
    }

    public class DataObjectWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public DataObjectWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is IDataObject;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateDataObject((IDataObject)obj);
    }

    public class HistoryItemWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public HistoryItemWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is IHistoryItem;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateHistoryItem((IHistoryItem)obj);
    }

    public class TypeWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public TypeWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is IType;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateType((IType)obj);
    }

    public class PersonWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public PersonWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is IPerson;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreatePerson((IPerson)obj);
    }

    public class UserStateWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public UserStateWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is IUserState;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateUserState((IUserState)obj);
    }

    public class UserStateMachineWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public UserStateMachineWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is IUserStateMachine;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateUserStateMachine((IUserStateMachine)obj);
    }

    public class AttributeWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public AttributeWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is IAttribute;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateAttribute((IAttribute)obj);
    }

    public class KeyValuePairStringObjectWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public KeyValuePairStringObjectWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is KeyValuePair<string, object>;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateKeyValuePair((KeyValuePair<string, object>)obj, (IDataObject)context.SenderObj.LookUpObject);
    }

    public class KeyValuePairGuidIntWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public KeyValuePairGuidIntWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is KeyValuePair<Guid, int>;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateKeyValuePair((KeyValuePair<Guid, int>)obj);
    }

    public class KeyValuePairDataObjectIntWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public KeyValuePairDataObjectIntWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is KeyValuePair<IDataObject, int>;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateKeyValuePair((KeyValuePair<IDataObject, int>)obj);
    }

    public class KeyValuePairGuidEnumerableTransitionWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public KeyValuePairGuidEnumerableTransitionWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is KeyValuePair<Guid, IEnumerable<ITransition>>;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateKeyValuePair((KeyValuePair<Guid, IEnumerable<ITransition>>)obj, context.ObjectsRepository);
    }

    public class KeyValuePairIntAccessWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public KeyValuePairIntAccessWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is KeyValuePair<int, IAccess>;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateKeyValuePair((KeyValuePair<int, IAccess>)obj);
    }

    public class RelationWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public RelationWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is IRelation;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateRelation((IRelation)obj);
    }

    public class FileWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public FileWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is IFile;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateFile((IFile)obj);
    }

    public class AccessWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public AccessWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is IAccess;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateAccess((IAccess)obj);
    }

    public class AccessRecordWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public AccessRecordWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is IAccessRecord;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateAccessRecord((IAccessRecord)obj, context.ObjectsRepository);
    }

    public class FilesSnapshotWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public FilesSnapshotWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is IFilesSnapshot;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateFilesSnapshot((IFilesSnapshot)obj);
    }

    public class PositionWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public PositionWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is IPosition;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreatePosition((IPosition)obj, context.ObjectsRepository);
    }

    public class OrganizationUnitWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public OrganizationUnitWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is IOrganisationUnit;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateOrganisationUnit((IOrganisationUnit)obj);
    }

    public class TransitionWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public TransitionWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is ITransition;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateTransition((ITransition)obj);
    }

    public class StorageDataObjectWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public StorageDataObjectWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is IStorageDataObject;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateStorageDataObject((IStorageDataObject)obj);
    }

    public class StateInfoWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public StateInfoWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is IStateInfo;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateStateInfo((IStateInfo)obj);
    }

    public class LockInfoWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public LockInfoWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is ILockInfo;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateLockInfo((ILockInfo)obj);
    }

    public class SignatureRequestWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public SignatureRequestWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is ISignatureRequest;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateSignatureRequest((ISignatureRequest)obj);
    }

    public class ReportItemWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public ReportItemWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is IReportItem;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateReportItem((IReportItem)obj);
    }

    public class SignatureWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public SignatureWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is ISignature;
        public IPilotObjectHelper Wrap(object obj, TypeWrapContext context) => _factory.CreateSignature((ISignature)obj);
    }
} 