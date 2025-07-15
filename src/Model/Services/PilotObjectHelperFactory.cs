using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Data;
using PilotLookUp.Domain.Entities.Helpers;
using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System;
using System.Collections.Generic;

namespace PilotLookUp.Model.Services
{
    public class PilotObjectHelperFactory : IPilotObjectHelperFactory
    {
        private readonly IThemeService _themeService;
        private readonly ILogger _logger;
        public PilotObjectHelperFactory(IThemeService themeService,
            ILogger logger)
        {
            _themeService = themeService;
            _logger = logger;
        }

        public IPilotObjectHelper CreateDefault(string name, string stringId, object lookUpObject, bool isLookable)
        {
            return new DefaultPilotObjectHelper(_themeService, name, stringId, lookUpObject, isLookable, _logger);
        }
        public IPilotObjectHelper CreateBool(bool value)
        {
            return new BoolHelper(_themeService, value, _logger);
        }
        public IPilotObjectHelper CreateInt(
            int value,
            IObjectsRepository objectsRepository,
            IPilotObjectHelper sender,
            System.Reflection.MemberInfo senderMember)
        {
            return new IntHelper(_themeService, value, objectsRepository, sender, senderMember, _logger);
        }
        public IPilotObjectHelper CreateString(string value)
        {
            return new StringHelper(_themeService, value, _logger);
        }
        public IPilotObjectHelper CreateEnum(System.Enum value)
        {
            return new EnumHelper(_themeService, value, _logger);
        }
        public IPilotObjectHelper CreateGuid(System.Guid value)
        {
            return new GuidHelper(_themeService, value, _logger);
        }
        public IPilotObjectHelper CreateLong(long value)
        {
            return new LongHelper(_themeService, value, _logger);
        }
        public IPilotObjectHelper CreateNull(object value = null)
        {
            return new NullHelper(_themeService, value, _logger);
        }
        public IPilotObjectHelper CreateAccess(IAccess obj)
        {
            return new AccessHelper(_themeService, obj, _logger);
        }
        public IPilotObjectHelper CreateType(IType obj) => new TypeHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreatePerson(IPerson obj) => new PersonHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreateUserState(IUserState obj) => new UserStateHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreateUserStateMachine(IUserStateMachine obj) => new UserStateMachineHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreateAttribute(IAttribute obj) => new AttributeHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreateKeyValuePair(object obj, object extra = null)
        {
            if (obj is KeyValuePair<string, object> kv1 && extra is IDataObject dataObj)
                return new KeyValuePairHelper(_themeService, kv1, dataObj, _logger);
            if (obj is KeyValuePair<Guid, int> kv2)
                return new KeyValuePairHelper(_themeService, kv2, _logger);
            if (obj is KeyValuePair<IDataObject, int> kv3)
                return new KeyValuePairHelper(_themeService, kv3, _logger);
            if (obj is KeyValuePair<Guid, IEnumerable<ITransition>> kv4 && extra is IObjectsRepository repo)
                return new KeyValuePairHelper(_themeService, kv4, repo, _logger);
            if (obj is KeyValuePair<int, IAccess> kv5)
                return new KeyValuePairHelper(_themeService, kv5, _logger);
            return null;
        }
        public IPilotObjectHelper CreateRelation(IRelation obj) => new RelationHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreateFile(IFile obj) => new FileHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreateAccessRecord(IAccessRecord obj, IObjectsRepository repo) => new AccessRecordHelper(_themeService, obj, repo, _logger);
        public IPilotObjectHelper CreateFilesSnapshot(IFilesSnapshot obj) => new FilesSnapshotHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreatePosition(IPosition obj) => new PositionHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreatePosition(IPosition obj, IObjectsRepository repo) => new PositionHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreateOrganisationUnit(IOrganisationUnit obj) => new OrganizationUnitHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreateTransition(ITransition obj) => new TransitionHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreateStorageDataObject(IStorageDataObject obj) => new StorageDataObjectHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreateStateInfo(IStateInfo obj) => new StateInfoHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreateLockInfo(ILockInfo obj) => new LockInfoHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreateSignatureRequest(ISignatureRequest obj) => new SignatureRequestHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreateReportItem(IReportItem obj) => new ReportItemHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreateSignature(ISignature obj) => new SignatureHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreateObjectsRepository(IObjectsRepository obj) => new ObjectsRepositoryHelper(_themeService, obj, _logger);
        public IPilotObjectHelper CreateDataObject(IDataObject obj)
        {
            return new DataObjectHelper(_themeService, obj, _logger);
        }
        public IPilotObjectHelper CreateDateTime(DateTime value)
        {
            return new DateTimeHelper(_themeService, value, _logger);
        }
        public IPilotObjectHelper CreateHistoryItem(IHistoryItem obj)
        {
            return new HistoryItemHelper(_themeService, obj, _logger);
        }
        public IPilotObjectHelper Create(string name, string stringId, object lookUpObject, bool isLookable)
        {
            return CreateDefault(name, stringId, lookUpObject, isLookable);
        }
    }
} 