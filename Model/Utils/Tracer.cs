using Ascon.Pilot.SDK;
using PilotLookUp.Extensions;
using PilotLookUp.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PilotLookUp.Model.Utils
{
    public class Tracer
    {
        public Tracer(IObjectsRepository objectsRepository, PilotObjectHelper senderObj, MemberInfo senderMember)
        {
            _pilotObjectMap = new PilotObjectMap(objectsRepository, senderObj, senderMember);
            _objectsRepository = objectsRepository;
            _objectSet = new ObjectSet(senderMember);
        }

        private int _adaptiveTimer = 200;
        private PilotObjectMap _pilotObjectMap { get; }
        private IObjectsRepository _objectsRepository { get; }
        private ObjectSet _objectSet { get; set; }
        private IEnumerable<IUserState> _userStates { get; set; }
        private IEnumerable<IUserState> _userStateMachines { get; set; }


        public async Task<ObjectSet> Trace(object obj)
        {
            IEnumerable<IUserState> a = _objectsRepository.GetUserStates();
            IEnumerable<IUserStateMachine> b = _objectsRepository.GetUserStateMachines();

            if (obj == null) return _objectSet;

            // Определение типа объекта и вызов соответствующей перегрузки
            if (obj is IEnumerable enumerable
                && !(obj is string))
            {
                return await AddToSelectionEnum(enumerable.Cast<object>());
            }
            else
            {
                return await AddToSelection(obj);
            }
        }

        private async Task<ObjectSet> AddToSelectionEnum<T>(IEnumerable<T> objects)
        {
            foreach (object obj in objects)
            {
                if (obj is Guid guid)
                {
                    //var lodedObj = await _objectsRepository.GetObjectWithTimeout(guid, _adaptiveTimer);
                    //if (lodedObj != null)
                    //{
                    //    _objectSet.Add(_pilotObjectMap.Wrap(lodedObj));
                    //}
                    //else
                    //{
                    //    _adaptiveTimer = 10;
                    //    _objectSet.Add(_pilotObjectMap.Wrap(guid));
                    //}
                    _objectSet.Add(await GuidLoder(guid));
                }
                else if (obj is KeyValuePair<Guid, int> keyVal)
                {
                    var lodetDict = new KeyValuePair<IDataObject, int>(await _objectsRepository.GetObject(keyVal.Key), keyVal.Value);
                    _objectSet.Add(_pilotObjectMap.Wrap(lodetDict));
                }
                else
                    _objectSet.Add(_pilotObjectMap.Wrap(obj));
            }
            return _objectSet;
        }

        private async Task<ObjectSet> AddToSelection(object obj)
        {
            if (obj is Guid guid)
            {
                //var lodedObj = await _objectsRepository.GetObjectWithTimeout(guid, _adaptiveTimer);
                //if (lodedObj != null)
                //{
                //    _objectSet.Add(_pilotObjectMap.Wrap(lodedObj));
                //}
                //else
                //{
                //    _objectSet.Add(_pilotObjectMap.Wrap(guid));
                //}
                _objectSet.Add(await GuidLoder(guid));
            }
            else
            {
                _objectSet.Add(_pilotObjectMap.Wrap(obj));
            }
            return _objectSet;
        }

        private async Task<PilotObjectHelper> GuidLoder(Guid guid)
        {
            var lodedObj = await _objectsRepository.GetObjectWithTimeout(guid, _adaptiveTimer);
            if (lodedObj != null)
            {
                return _pilotObjectMap.Wrap(lodedObj);
            }

            var lodedHistory = await _objectsRepository.GetHistoryItemWithTimeout(guid, _adaptiveTimer);
            if (lodedHistory != null)
            {
                return _pilotObjectMap.Wrap(lodedObj);
            }

            _adaptiveTimer = 10;
            return _pilotObjectMap.Wrap(guid);
        }
    }
}
