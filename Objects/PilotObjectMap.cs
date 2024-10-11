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
        public static PilotObjectHelper Wrap(object obj, object sender = null)
        {

            return obj switch
            {
                //System
                //string value when type is null || type == typeof(string) => new StringDescriptor(value),
                //bool value when type is null || type == typeof(bool) => new BoolDescriptor(value),
                //MacroManager when type is null || type == typeof(MacroManager) => new MacroManagerDescriptor(),
                //IEnumerable value => new EnumerableDescriptor(value),
                //Exception value when type is null || type == typeof(Exception) => new ExceptionDescriptor(value),
                Guid value => new GuidHelper(value, _objectsRepository),

                // PilotTypes
                IDataObject value => new DataObjectHelper(value),
                IType type => new TypeHelper(type),
                IPerson value => new PersonHelper(value),
                IUserState value => new UserStateHelper(value),
                IUserStateMachine value => new UserStateMachineHelper(value),
                IAttribute value => new AttributeHelper(value),
                KeyValuePair<string, object> value => new KeyValuePairHelper(value, (IDataObject)sender),
                KeyValuePair<Guid, int> value => new KeyValuePairHelper(value),
                KeyValuePair<Guid, IEnumerable<ITransition>> value => new KeyValuePairHelper(value, _objectsRepository),
                IRelation value => new RelationHelper(value),
                IFile value => new FileHelper(value),
                IAccess value => new AccessHelper(value),
                IAccessRecord value => new AccessRecordHelper(value, _objectsRepository),
                IFilesSnapshot value => new FilesSnapshotHelper(value),
                IPosition value => new PositionHelper(value, _objectsRepository),
                IOrganisationUnit value => new OrganisationUnitHelper(value),
                ITransition value => new TransitionHelper(value),
                IStorageDataObject value => new StorageDataObjectHelper(value),

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
