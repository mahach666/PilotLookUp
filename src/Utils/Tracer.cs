using Ascon.Pilot.SDK;
using PilotLookUp.Extensions;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PilotLookUp.Utils
{
    public class Tracer
    {
        public Tracer(IObjectsRepository objectsRepository, PilotObjectHelper senderObj, MemberInfo senderMember, IObjectSetFactory objectSetFactory)
        {
            _pilotObjectMap = new PilotObjectMap(objectsRepository, senderObj, senderMember);
            _objectsRepository = objectsRepository;
            _memberInfo = senderMember;
            _objectSet = objectSetFactory.Create(senderMember);
        }

        private int _adaptiveTimer = 200;
        private PilotObjectMap _pilotObjectMap { get; }
        private IObjectsRepository _objectsRepository { get; }
        private ObjectSet _objectSet { get; set; }
        private MemberInfo _memberInfo { get; }
        private IEnumerable<IUserState> _userStates { get; set; }
        private IEnumerable<IUserStateMachine> _userStateMachines { get; set; }


        public async Task<ObjectSet> Trace(object obj)
        {
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
                    _userStates = _objectsRepository.GetUserStates();
                    _userStateMachines = _objectsRepository.GetUserStateMachines();
                    _objectSet.Add(await GuidHandler(guid));
                }
                else if (obj is KeyValuePair<Guid, int> keyVal)
                {
                    var lodetDict = new KeyValuePair<IDataObject, int>(await _objectsRepository.GetObjectWithTimeout(keyVal.Key), keyVal.Value);
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
                _userStates = _objectsRepository.GetUserStates();
                _userStateMachines = _objectsRepository.GetUserStateMachines();
                _objectSet.Add(await GuidHandler(guid));
            }
            else
            {
                _objectSet.Add(_pilotObjectMap.Wrap(obj));
            }
            return _objectSet;
        }

        private async Task<PilotObjectHelper> GuidHandler(Guid guid)
        {
            if (_memberInfo?.Name== "HistoryItems")
            {
                var lodedHistory = await _objectsRepository.GetHistoryItemWithTimeout(guid, _adaptiveTimer);
                if (lodedHistory != null)
                {
                    return _pilotObjectMap.Wrap(lodedHistory);
                } 
            }

            var lodedObj = await _objectsRepository.GetObjectWithTimeout(guid, _adaptiveTimer);
            if (lodedObj != null)
            {
                return _pilotObjectMap.Wrap(lodedObj);
            }

            _adaptiveTimer = 10;

            var userState = _userStates.FirstOrDefault(i => i.Id == guid);
            if (userState != null)
            {
                return _pilotObjectMap.Wrap(userState);
            }

            var userStateMachine = _userStateMachines.FirstOrDefault(i => i.Id == guid);
            if (userStateMachine != null)
            {
                return _pilotObjectMap.Wrap(userStateMachine);
            }

            return _pilotObjectMap.Wrap(guid);
        }
    }
}
