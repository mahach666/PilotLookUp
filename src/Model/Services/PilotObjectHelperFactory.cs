using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Data;
using System;
using System.Collections.Generic;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.Objects.TypeHelpers;

namespace PilotLookUp.Model.Services
{
    public class PilotObjectHelperFactory : IPilotObjectHelperFactory
    {
        private readonly IThemeService _themeService;
        public PilotObjectHelperFactory(IThemeService themeService)
        {
            _themeService = themeService;
        }

        public IPilotObjectHelper CreateDefault(string name, string stringId, object lookUpObject, bool isLookable)
        {
            return new DefaultPilotObjectHelper(_themeService, name, stringId, lookUpObject, isLookable);
        }
        public IPilotObjectHelper CreateBool(bool value)
        {
            return new BoolHelper(_themeService, value);
        }
        public IPilotObjectHelper CreateInt(int value, IObjectsRepository objectsRepository, IPilotObjectHelper sender, System.Reflection.MemberInfo senderMember)
        {
            return new IntHelper(_themeService, value, objectsRepository, sender, senderMember);
        }
        public IPilotObjectHelper CreateString(string value)
        {
            return new StringHelper(_themeService, value);
        }
        public IPilotObjectHelper CreateEnum(System.Enum value)
        {
            return new EnumHelper(_themeService, value);
        }
        public IPilotObjectHelper CreateGuid(System.Guid value)
        {
            return new GuidHelper(_themeService, value);
        }
        public IPilotObjectHelper CreateLong(long value)
        {
            return new LongHelper(_themeService, value);
        }
        public IPilotObjectHelper CreateNull(object value = null)
        {
            return new NullHelper(_themeService, value);
        }
        public IPilotObjectHelper CreateAccess(IAccess obj)
        {
            return new AccessHelper(_themeService, obj);
        }
        public IPilotObjectHelper CreateType(IType obj) => new TypeHelper(_themeService, obj);
        public IPilotObjectHelper CreatePerson(IPerson obj) => new PersonHelper(_themeService, obj);
        public IPilotObjectHelper CreateUserState(IUserState obj) => new UserStateHelper(_themeService, obj);
        public IPilotObjectHelper CreateUserStateMachine(IUserStateMachine obj) => new UserStateMachineHelper(_themeService, obj);
        public IPilotObjectHelper CreateAttribute(IAttribute obj) => new AttributeHelper(_themeService, obj);
        public IPilotObjectHelper CreateKeyValuePair(object obj, object extra = null)
        {
            // Здесь можно реализовать логику выбора нужного KeyValuePairHelper
            // Например:
            if (obj is KeyValuePair<string, object> kv1 && extra is IDataObject dataObj)
                return new KeyValuePairHelper(_themeService, kv1, dataObj);
            if (obj is KeyValuePair<Guid, int> kv2)
                return new KeyValuePairHelper(_themeService, kv2);
            if (obj is KeyValuePair<IDataObject, int> kv3)
                return new KeyValuePairHelper(_themeService, kv3);
            if (obj is KeyValuePair<Guid, IEnumerable<ITransition>> kv4 && extra is IObjectsRepository repo)
                return new KeyValuePairHelper(_themeService, kv4, repo);
            if (obj is KeyValuePair<int, IAccess> kv5)
                return new KeyValuePairHelper(_themeService, kv5);
            return null;
        }
        public IPilotObjectHelper CreateRelation(IRelation obj) => new RelationHelper(_themeService, obj);
        public IPilotObjectHelper CreateFile(IFile obj) => new FileHelper(_themeService, obj);
        public IPilotObjectHelper CreateAccessRecord(IAccessRecord obj, IObjectsRepository repo) => new AccessRecordHelper(_themeService, obj, repo);
        public IPilotObjectHelper CreateFilesSnapshot(IFilesSnapshot obj) => new FilesSnapshotHelper(_themeService, obj);
        public IPilotObjectHelper CreatePosition(IPosition obj) => new PositionHelper(_themeService, obj);
        public IPilotObjectHelper CreatePosition(IPosition obj, IObjectsRepository repo) => new PositionHelper(_themeService, obj);
        public IPilotObjectHelper CreateOrganisationUnit(IOrganisationUnit obj) => new OrganizationUnitHelper(_themeService, obj);
        public IPilotObjectHelper CreateTransition(ITransition obj) => new TransitionHelper(_themeService, obj);
        public IPilotObjectHelper CreateStorageDataObject(IStorageDataObject obj) => new StorageDataObjectHelper(_themeService, obj);
        public IPilotObjectHelper CreateStateInfo(IStateInfo obj) => new StateInfoHelper(_themeService, obj);
        public IPilotObjectHelper CreateLockInfo(ILockInfo obj) => new LockInfoHelper(_themeService, obj);
        public IPilotObjectHelper CreateSignatureRequest(ISignatureRequest obj) => new SignatureRequestHelper(_themeService, obj);
        public IPilotObjectHelper CreateReportItem(IReportItem obj) => new ReportItemHelper(_themeService, obj);
        public IPilotObjectHelper CreateSignature(ISignature obj) => new SignatureHelper(_themeService, obj);
        public IPilotObjectHelper CreateObjectsRepository(IObjectsRepository obj) => new ObjectsRepositoryHelper(_themeService, obj);
        public IPilotObjectHelper CreateDataObject(IDataObject obj)
        {
            return new DataObjectHelper(_themeService, obj);
        }
        public IPilotObjectHelper CreateDateTime(DateTime value)
        {
            return new DateTimeHelper(_themeService, value);
        }
        public IPilotObjectHelper CreateHistoryItem(IHistoryItem obj)
        {
            return new HistoryItemHelper(_themeService, obj);
        }
        public IPilotObjectHelper Create(string name, string stringId, object lookUpObject, bool isLookable)
        {
            return CreateDefault(name, stringId, lookUpObject, isLookable);
        }
    }
} 