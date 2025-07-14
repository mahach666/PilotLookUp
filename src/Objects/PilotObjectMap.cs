using Ascon.Pilot.SDK;
using PilotLookUp.Objects.Strategies;
using PilotLookUp.Objects.TypeHelpers;
using System.Reflection;

namespace PilotLookUp.Objects
{
    public class PilotObjectMap
    {
        private readonly TypeWrapStrategyRegistry _strategyRegistry;
        public PilotObjectMap(IObjectsRepository objectsRepository, PilotObjectHelper senderObj = null, MemberInfo senderMember = null)
        {
            _objectsRepository = objectsRepository;
            _senderObj = senderObj;
            _senderMember = senderMember;
            _strategyRegistry = new TypeWrapStrategyRegistry();
            RegisterDefaultStrategies(_strategyRegistry);
        }
        private IObjectsRepository _objectsRepository { get; }
        private PilotObjectHelper _senderObj { get; }
        private MemberInfo _senderMember { get; }

        public PilotObjectHelper Wrap(object obj)
        {
            var context = new TypeWrapContext(_objectsRepository, _senderObj, _senderMember);
            try
            {
                return _strategyRegistry.Wrap(obj, context);
            }
            catch
            {
                return new OtherHelper(obj);
            }
        }

        private void RegisterDefaultStrategies(TypeWrapStrategyRegistry registry)
        {
            // Примитивы
            registry.Register(new StringWrapStrategy());
            registry.Register(new IntWrapStrategy());
            registry.Register(new BoolWrapStrategy());
            registry.Register(new LongWrapStrategy());
            registry.Register(new DateTimeWrapStrategy());
            registry.Register(new EnumWrapStrategy());
            registry.Register(new GuidWrapStrategy());
            registry.Register(new NullWrapStrategy());
            // Pilot-объекты
            registry.Register(new ObjectsRepositoryWrapStrategy());
            registry.Register(new DataObjectWrapStrategy());
            registry.Register(new HistoryItemWrapStrategy());
            registry.Register(new TypeWrapStrategy());
            registry.Register(new PersonWrapStrategy());
            registry.Register(new UserStateWrapStrategy());
            registry.Register(new UserStateMachineWrapStrategy());
            registry.Register(new AttributeWrapStrategy());
            registry.Register(new KeyValuePairStringObjectWrapStrategy());
            registry.Register(new KeyValuePairGuidIntWrapStrategy());
            registry.Register(new KeyValuePairDataObjectIntWrapStrategy());
            registry.Register(new KeyValuePairGuidEnumerableTransitionWrapStrategy());
            registry.Register(new KeyValuePairIntAccessWrapStrategy());
            registry.Register(new RelationWrapStrategy());
            registry.Register(new FileWrapStrategy());
            registry.Register(new AccessWrapStrategy());
            registry.Register(new AccessRecordWrapStrategy());
            registry.Register(new FilesSnapshotWrapStrategy());
            registry.Register(new PositionWrapStrategy());
            registry.Register(new OrganizationUnitWrapStrategy());
            registry.Register(new TransitionWrapStrategy());
            registry.Register(new StorageDataObjectWrapStrategy());
            registry.Register(new StateInfoWrapStrategy());
            registry.Register(new LockInfoWrapStrategy());
            registry.Register(new SignatureRequestWrapStrategy());
            registry.Register(new ReportItemWrapStrategy());
            registry.Register(new SignatureWrapStrategy());
        }

        public static PilotObjectHelper WrapNull()
        {
            return new NullHelper();
        }
    }
}
