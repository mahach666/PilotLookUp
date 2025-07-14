using Ascon.Pilot.SDK;
using PilotLookUp.Objects.TypeHelpers;
using System;

namespace PilotLookUp.Objects.Strategies
{
    public class StringWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is string;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new StringHelper((string)obj);
    }

    public class IntWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is int;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new IntHelper((int)obj, context.ObjectsRepository, context.SenderObj, context.SenderMember);
    }

    public class BoolWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is bool;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new BoolHelper((bool)obj);
    }

    public class LongWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is long;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new LongHelper((long)obj);
    }

    public class DateTimeWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is DateTime;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new DateTimeHelper((DateTime)obj);
    }

    public class EnumWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is Enum;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new EnumHelper((Enum)obj);
    }

    public class GuidWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj is Guid;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new GuidHelper((Guid)obj, context.SenderObj, context.SenderMember);
    }

    public class NullWrapStrategy : ITypeWrapStrategy
    {
        public bool CanWrap(object obj) => obj == null;
        public PilotObjectHelper Wrap(object obj, TypeWrapContext context) => new NullHelper();
    }
} 