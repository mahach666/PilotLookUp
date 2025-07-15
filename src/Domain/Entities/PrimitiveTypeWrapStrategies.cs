using PilotLookUp.Domain.Interfaces;
using System;

namespace PilotLookUp.Domain.Entities
{
    public class StringWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public StringWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is string;
        public IPilotObjectHelper Wrap(object obj,
            TypeWrapContext context) => _factory.CreateString((string)obj);
    }

    public class IntWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public IntWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is int;
        public IPilotObjectHelper Wrap(object obj,
            TypeWrapContext context) => _factory.CreateInt((int)obj,
                context.ObjectsRepository, context.SenderObj,
                context.SenderMember);
    }

    public class BoolWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public BoolWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is bool;
        public IPilotObjectHelper Wrap(object obj,
            TypeWrapContext context) => _factory.CreateBool((bool)obj);
    }

    public class LongWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public LongWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is long;
        public IPilotObjectHelper Wrap(object obj,
            TypeWrapContext context) => _factory.CreateLong((long)obj);
    }

    public class DateTimeWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public DateTimeWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is DateTime;
        public IPilotObjectHelper Wrap(object obj,
            TypeWrapContext context) => _factory.CreateDateTime((DateTime)obj);
    }

    public class EnumWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public EnumWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is Enum;
        public IPilotObjectHelper Wrap(object obj,
            TypeWrapContext context) => _factory.CreateEnum((Enum)obj);
    }

    public class GuidWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public GuidWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj is Guid;
        public IPilotObjectHelper Wrap(object obj,
            TypeWrapContext context) => _factory.CreateGuid((Guid)obj);
    }

    public class NullWrapStrategy : ITypeWrapStrategy
    {
        private readonly IPilotObjectHelperFactory _factory;
        public NullWrapStrategy(IPilotObjectHelperFactory factory) { _factory = factory; }
        public bool CanWrap(object obj) => obj == null;
        public IPilotObjectHelper Wrap(object obj,
            TypeWrapContext context) => _factory.CreateNull();
    }
} 