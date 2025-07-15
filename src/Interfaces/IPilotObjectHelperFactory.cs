using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Data;
using System;

namespace PilotLookUp.Interfaces
{
    public interface IPilotObjectHelperFactory
    {
        IPilotObjectHelper Create(string name, string stringId, object lookUpObject, bool isLookable);
        IPilotObjectHelper CreateDefault(string name, string stringId, object lookUpObject, bool isLookable);
        IPilotObjectHelper CreateBool(bool value);
        IPilotObjectHelper CreateInt(int value, IObjectsRepository objectsRepository, IPilotObjectHelper sender, System.Reflection.MemberInfo senderMember);
        IPilotObjectHelper CreateString(string value);
        IPilotObjectHelper CreateEnum(System.Enum value);
        IPilotObjectHelper CreateGuid(System.Guid value);
        IPilotObjectHelper CreateLong(long value);
        IPilotObjectHelper CreateNull(object value = null);
        IPilotObjectHelper CreateAccess(IAccess obj);
        IPilotObjectHelper CreateType(IType obj);
        IPilotObjectHelper CreatePerson(IPerson obj);
        IPilotObjectHelper CreateUserState(IUserState obj);
        IPilotObjectHelper CreateUserStateMachine(IUserStateMachine obj);
        IPilotObjectHelper CreateAttribute(IAttribute obj);
        IPilotObjectHelper CreateKeyValuePair(object obj, object extra = null);
        IPilotObjectHelper CreateRelation(IRelation obj);
        IPilotObjectHelper CreateFile(IFile obj);
        IPilotObjectHelper CreateAccessRecord(IAccessRecord obj, IObjectsRepository repo);
        IPilotObjectHelper CreateFilesSnapshot(IFilesSnapshot obj);
        IPilotObjectHelper CreatePosition(IPosition obj, IObjectsRepository repo);
        IPilotObjectHelper CreateOrganisationUnit(IOrganisationUnit obj);
        IPilotObjectHelper CreateTransition(ITransition obj);
        IPilotObjectHelper CreateStorageDataObject(IStorageDataObject obj);
        IPilotObjectHelper CreateStateInfo(IStateInfo obj);
        IPilotObjectHelper CreateLockInfo(ILockInfo obj);
        IPilotObjectHelper CreateSignatureRequest(ISignatureRequest obj);
        IPilotObjectHelper CreateReportItem(IReportItem obj);
        IPilotObjectHelper CreateSignature(ISignature obj);
        IPilotObjectHelper CreateObjectsRepository(IObjectsRepository obj);
        IPilotObjectHelper CreateDataObject(IDataObject obj);
        IPilotObjectHelper CreateDateTime(DateTime value);
    }
} 