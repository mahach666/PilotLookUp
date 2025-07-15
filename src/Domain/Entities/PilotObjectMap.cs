using Ascon.Pilot.SDK;
using PilotLookUp.Objects.Strategies;
using PilotLookUp.Domain.Entities;
using System.Reflection;
using PilotLookUp.Domain.Interfaces;

namespace PilotLookUp.Domain.Entities
{
    public class PilotObjectMap
    {
        private readonly TypeWrapStrategyRegistry _strategyRegistry;
        private readonly IPilotObjectHelperFactory _factory;
        public PilotObjectMap(IObjectsRepository objectsRepository, IPilotObjectHelperFactory factory, IPilotObjectHelper senderObj = null, System.Reflection.MemberInfo senderMember = null)
        {
            _objectsRepository = objectsRepository;
            _factory = factory;
            _senderObj = senderObj;
            _senderMember = senderMember;
            _strategyRegistry = new TypeWrapStrategyRegistry();
            RegisterDefaultStrategies(_strategyRegistry);
        }
        private IObjectsRepository _objectsRepository { get; }
        private IPilotObjectHelper _senderObj { get; }
        private System.Reflection.MemberInfo _senderMember { get; }

        public IPilotObjectHelper Wrap(object obj)
        {
            var context = new TypeWrapContext(_objectsRepository, _senderObj, _senderMember);
            try
            {
                return _strategyRegistry.Wrap(obj, context);
            }
            catch
            {
                return _factory.CreateDefault(PilotLookUp.Resources.Strings.Unknown, PilotLookUp.Resources.Strings.Unknown, obj, false);
            }
        }

        private void RegisterDefaultStrategies(TypeWrapStrategyRegistry registry)
        {
            // Примитивы
            registry.Register(new StringWrapStrategy(_factory));
            registry.Register(new IntWrapStrategy(_factory));
            registry.Register(new BoolWrapStrategy(_factory));
            registry.Register(new LongWrapStrategy(_factory));
            registry.Register(new DateTimeWrapStrategy(_factory));
            registry.Register(new EnumWrapStrategy(_factory));
            registry.Register(new GuidWrapStrategy(_factory));
            registry.Register(new NullWrapStrategy(_factory));
            // Pilot-объекты
            registry.Register(new ObjectsRepositoryWrapStrategy(_factory));
            registry.Register(new DataObjectWrapStrategy(_factory));
            registry.Register(new HistoryItemWrapStrategy(_factory));
            registry.Register(new TypeWrapStrategy(_factory));
            registry.Register(new PersonWrapStrategy(_factory));
            registry.Register(new UserStateWrapStrategy(_factory));
            registry.Register(new UserStateMachineWrapStrategy(_factory));
            registry.Register(new AttributeWrapStrategy(_factory));
            registry.Register(new KeyValuePairStringObjectWrapStrategy(_factory));
            registry.Register(new KeyValuePairGuidIntWrapStrategy(_factory));
            registry.Register(new KeyValuePairDataObjectIntWrapStrategy(_factory));
            registry.Register(new KeyValuePairGuidEnumerableTransitionWrapStrategy(_factory));
            registry.Register(new KeyValuePairIntAccessWrapStrategy(_factory));
            registry.Register(new RelationWrapStrategy(_factory));
            registry.Register(new FileWrapStrategy(_factory));
            registry.Register(new AccessWrapStrategy(_factory));
            registry.Register(new AccessRecordWrapStrategy(_factory));
            registry.Register(new FilesSnapshotWrapStrategy(_factory));
            registry.Register(new PositionWrapStrategy(_factory));
            registry.Register(new OrganizationUnitWrapStrategy(_factory));
            registry.Register(new TransitionWrapStrategy(_factory));
            registry.Register(new StorageDataObjectWrapStrategy(_factory));
            registry.Register(new StateInfoWrapStrategy(_factory));
            registry.Register(new LockInfoWrapStrategy(_factory));
            registry.Register(new SignatureRequestWrapStrategy(_factory));
            registry.Register(new ReportItemWrapStrategy(_factory));
            registry.Register(new SignatureWrapStrategy(_factory));
        }

        public static IPilotObjectHelper WrapNull()
        {
            return null; // Это статический метод, нужно будет исправить по-другому
        }
    }
}
