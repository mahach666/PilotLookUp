using Ascon.Pilot.SDK;
using PilotLookUp.Model;
using PilotLookUp.Objects.TypeHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.Objects
{
    internal static class PilotObjectMap
    {
        public static PilotObjectHelper Wrap(object obj, PilotObjectHelper sender = null)
        {

            return obj switch
            {
                //System
                int value =>  new IntHelper(value, _objectsRepository, sender),
                string value => new StringHelper(value, _objectsRepository),
                //bool value when type is null || type == typeof(bool) => new BoolDescriptor(value),
                //MacroManager when type is null || type == typeof(MacroManager) => new MacroManagerDescriptor(),
                //IEnumerable value => new EnumerableDescriptor(value),
                //Exception value when type is null || type == typeof(Exception) => new ExceptionDescriptor(value),
                //Guid value => new GuidHelper(value, _objectsRepository),

                // PilotTypes
                IDataObject value => new DataObjectHelper(value, _objectsRepository),
                IType type => new TypeHelper(type, _objectsRepository),
                IPerson value => new PersonHelper(value, _objectsRepository),
                IUserState value => new UserStateHelper(value, _objectsRepository),
                IUserStateMachine value => new UserStateMachineHelper(value, _objectsRepository),
                IAttribute value => new AttributeHelper(value, _objectsRepository),
                KeyValuePair<string, object> value => new KeyValuePairHelper(value, _objectsRepository,(IDataObject)sender.LookUpObject),
                KeyValuePair<Guid, int> value => new KeyValuePairHelper(value, _objectsRepository),
                KeyValuePair<Guid, IEnumerable<ITransition>> value => new KeyValuePairHelper(value, _objectsRepository),
                IRelation value => new RelationHelper(value, _objectsRepository),
                IFile value => new FileHelper(value, _objectsRepository),
                IAccess value => new AccessHelper(value, _objectsRepository),
                IAccessRecord value => new AccessRecordHelper(value, _objectsRepository),
                IFilesSnapshot value => new FilesSnapshotHelper(value, _objectsRepository),
                IPosition value => new PositionHelper(value, _objectsRepository),
                IOrganisationUnit value => new OrganisationUnitHelper(value, _objectsRepository),
                ITransition value => new TransitionHelper(value, _objectsRepository),
                IStorageDataObject value => new StorageDataObjectHelper(value, _objectsRepository),
                null => null, //new NullHelper(),

                _ => null
            };
        }

        public static void Updaate(IObjectsRepository objectsRepository)
        {
            _objectsRepository = objectsRepository;
        }
        private static IObjectsRepository _objectsRepository { get; set; }
    }
}
