using Ascon.Pilot.SDK;
using PilotLookUp.Extensions;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.Objects.TypeHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PilotLookUp.Utils
{
    public class Tracer
    {
        private static readonly Regex GuidRegex = new Regex(
            @"[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}",
            RegexOptions.Compiled);

        public Tracer(IObjectsRepository objectsRepository, PilotObjectHelper senderObj, MemberInfo senderMember,
            IBimModelService bimModelService = null)
        {
            _pilotObjectMap = new PilotObjectMap(objectsRepository, senderObj, senderMember);
            _objectsRepository = objectsRepository;
            _senderObj = senderObj;
            _memberInfo = senderMember;
            _bimModelService = bimModelService;
            _objectSet = new ObjectSet(senderMember);
        }

        private int _adaptiveTimer = 200;
        private PilotObjectMap _pilotObjectMap { get; }
        private IObjectsRepository _objectsRepository { get; }
        private PilotObjectHelper _senderObj { get; }
        private IBimModelService _bimModelService { get; }
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
                else if (TryParseElementBookGuidsFromString(obj, out var parsedGuids))
                {
                    _userStates = _objectsRepository.GetUserStates();
                    _userStateMachines = _objectsRepository.GetUserStateMachines();
                    foreach (var parsedGuid in parsedGuids)
                        _objectSet.Add(await GuidHandler(parsedGuid));
                }
                else if (obj is KeyValuePair<Guid, int> keyVal)
                {
                    var lodetDict = new KeyValuePair<IDataObject, int>(await _objectsRepository.GetObjectWithTimeout(keyVal.Key), keyVal.Value);
                    _objectSet.Add(_pilotObjectMap.Wrap(lodetDict));
                }
                else if (obj is Ascon.Pilot.Bim.SDK.IModelElementId modelElementId)
                {
                    _objectSet.Add(WrapModelElementId(modelElementId));
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
            else if (TryParseAttributeConfigGuidsFromString(obj, out var originalText, out var configGuids))
            {
                _userStates = _objectsRepository.GetUserStates();
                _userStateMachines = _objectsRepository.GetUserStateMachines();

                foreach (var parsedGuid in configGuids)
                    _objectSet.Add(await GuidHandler(parsedGuid));

                _objectSet.DescriptionOverride = originalText;
            }
            else if (TryParseElementBookGuidsFromString(obj, out var parsedGuids))
            {
                _userStates = _objectsRepository.GetUserStates();
                _userStateMachines = _objectsRepository.GetUserStateMachines();
                foreach (var parsedGuid in parsedGuids)
                    _objectSet.Add(await GuidHandler(parsedGuid));
            }
            else if (obj is Ascon.Pilot.Bim.SDK.IModelElementId modelElementId)
            {
                _objectSet.Add(WrapModelElementId(modelElementId));
            }
            else
            {
                _objectSet.Add(_pilotObjectMap.Wrap(obj));
            }
            return _objectSet;
        }

        // Id элемента модели резолвим в элемент со свойствами, если модель загружена
        private PilotObjectHelper WrapModelElementId(Ascon.Pilot.Bim.SDK.IModelElementId id)
        {
            var view = _bimModelService?.TryLoadElementView(id);
            return _pilotObjectMap.Wrap(view ?? (object)id);
        }

        private bool TryParseAttributeConfigGuidsFromString(object obj, out string originalText, out List<Guid> guids)
        {
            originalText = null;
            guids = null;

            if (_senderObj?.LookUpObject is not IAttribute)
                return false;

            if (_memberInfo?.Name != "Configuration" && _memberInfo?.Name != "Configuration2")
                return false;

            if (obj is not string text || string.IsNullOrWhiteSpace(text))
                return false;

            var matches = GuidRegex.Matches(text);
            if (matches.Count == 0)
                return false;

            var parsed = new List<Guid>();
            foreach (Match match in matches)
            {
                if (Guid.TryParse(match.Value, out var guid))
                    parsed.Add(guid);
            }

            if (parsed.Count == 0)
                return false;

            originalText = text;
            guids = parsed.Distinct().ToList();
            return true;
        }

        private bool TryParseElementBookGuidsFromString(object obj, out List<Guid> guids)
        {
            guids = null;

            if (_memberInfo?.Name != "Value")
                return false;

            if (_senderObj is not KeyValuePairHelper keyValuePairHelper)
                return false;

            if (keyValuePairHelper.AttrType != AttributeType.ElementBook)
                return false;

            if (obj is not string s || string.IsNullOrWhiteSpace(s))
                return false;

            var tokens = s.Split(new[] { ';', ',', '\n', '\r', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var parsed = new List<Guid>();
            foreach (var token in tokens)
            {
                if (Guid.TryParse(token.Trim(), out var guid))
                    parsed.Add(guid);
            }

            if (parsed.Count == 0)
                return false;

            guids = parsed.Distinct().ToList();
            return true;
        }

        private async Task<PilotObjectHelper> GuidHandler(Guid guid)
        {
            if (_memberInfo?.Name== "HistoryItems")
            {
                try
                {
                    var lodedHistory = await _objectsRepository.GetHistoryItemWithTimeout(guid, _adaptiveTimer);
                    if (lodedHistory != null)
                    {
                        return _pilotObjectMap.Wrap(lodedHistory);
                    }
                }
                catch
                {
                }
            }

            try
            {
                var loadedObj = await _objectsRepository.GetObjectWithTimeout(guid, _adaptiveTimer);
                if (loadedObj != null)
                {
                    return _pilotObjectMap.Wrap(loadedObj);
                }
            }
            catch
            {
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

            var bimElement = TryResolveBimGuid(guid);
            if (bimElement != null)
            {
                return _pilotObjectMap.Wrap(bimElement);
            }

            return _pilotObjectMap.Wrap(guid);
        }

        // Голые Guid'ы ElementId/ParentId у BIM-объектов: контекст модели берем у отправителя
        private object TryResolveBimGuid(Guid guid)
        {
            if (_bimModelService == null)
                return null;

            try
            {
                var sender = _senderObj?.LookUpObject;
                if (sender == null)
                    return null;

                if (sender is BimElementView view)
                {
                    if (view.Id?.ElementId == guid)
                        return view;

                    var viewParentId = view.Element?.ParentId;
                    if (viewParentId != null && viewParentId.ElementId == guid)
                        return _bimModelService.TryLoadElementView(viewParentId);
                }

                if (sender is Ascon.Pilot.Bim.SDK.IModelElement element)
                {
                    if (element.Id?.ElementId == guid)
                    {
                        var resolved = _bimModelService.TryLoadElementView(element.Id);
                        if (resolved != null)
                            return resolved;
                    }
                    if (element.ParentId?.ElementId == guid)
                    {
                        var resolved = _bimModelService.TryLoadElementView(element.ParentId);
                        if (resolved != null)
                            return resolved;
                    }
                }

                if (sender is Ascon.Pilot.Bim.SDK.IModelElementId senderId && senderId.ElementId == guid)
                {
                    var resolved = _bimModelService.TryLoadElementView(senderId);
                    if (resolved != null)
                        return resolved;
                }

                // Внутренние типы вьюера (ObjectNodeViewModel и пр.) SDK-интерфейсы
                // не реализуют, но несут свойства ElementId/ParentId/ModelPartId -
                // читаем их рефлексией и резолвим через собственный id
                var modelPartId = ReadGuidProperty(sender, "ModelPartId");
                if (modelPartId.HasValue)
                {
                    var elementId = ReadGuidProperty(sender, "ElementId");
                    var parentGuid = ReadGuidProperty(sender, "ParentId");

                    if (elementId == guid || parentGuid == guid)
                        return _bimModelService.TryLoadElementView(guid, modelPartId.Value);
                }
            }
            catch
            {
                // Модель могла выгрузиться - оставляем голый Guid
            }

            return null;
        }

        private static Guid? ReadGuidProperty(object obj, string propertyName)
        {
            try
            {
                // GetProperty(name) может бросить AmbiguousMatchException на перекрытых
                // свойствах внутренних типов - ищем перебором
                var property = obj.GetType()
                    .GetProperties()
                    .FirstOrDefault(p => p.Name == propertyName && p.PropertyType == typeof(Guid));
                return property?.GetValue(obj) is Guid guid ? guid : (Guid?)null;
            }
            catch
            {
                return null;
            }
        }
    }
}
