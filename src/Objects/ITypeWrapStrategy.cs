using System;
using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Objects
{
    public interface ITypeWrapStrategy
    {
        bool CanWrap(object obj);
        IPilotObjectHelper Wrap(object obj, TypeWrapContext context);
    }

    public class TypeWrapContext
    {
        public TypeWrapContext(IObjectsRepository objectsRepository, IPilotObjectHelper senderObj, System.Reflection.MemberInfo senderMember)
        {
            ObjectsRepository = objectsRepository;
            SenderObj = senderObj;
            SenderMember = senderMember;
        }
        public IObjectsRepository ObjectsRepository { get; }
        public IPilotObjectHelper SenderObj { get; }
        public System.Reflection.MemberInfo SenderMember { get; }
    }
} 