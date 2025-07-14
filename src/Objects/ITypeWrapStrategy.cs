using System;
using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects
{
    public interface ITypeWrapStrategy
    {
        bool CanWrap(object obj);
        PilotObjectHelper Wrap(object obj, TypeWrapContext context);
    }

    public class TypeWrapContext
    {
        public TypeWrapContext(IObjectsRepository objectsRepository, PilotObjectHelper senderObj, System.Reflection.MemberInfo senderMember)
        {
            ObjectsRepository = objectsRepository;
            SenderObj = senderObj;
            SenderMember = senderMember;
        }
        public IObjectsRepository ObjectsRepository { get; }
        public PilotObjectHelper SenderObj { get; }
        public System.Reflection.MemberInfo SenderMember { get; }
    }
} 