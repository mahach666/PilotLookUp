using Ascon.Pilot.Bim.SDK;
using Ascon.Pilot.Bim.SDK.ModelTab.Menu;
using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.Model.Services
{
    public class SelectedService : ISelectedService
    {
        private IObjectsRepository _objectsRepository;
        private IClientEventService _clientEventService;
        private ObjectSet _convertSelection;
        private string _lastRaisedSelection;

        public SelectedService(IObjectsRepository objectsRepository, IClientEventService clientEventService)
        {
            _objectsRepository = objectsRepository;
            _clientEventService = clientEventService;
        }

        public ObjectSet Selected => _convertSelection;

        public void UpdateSelected(MarshalByRefObject context, string source = null)
        {
            try
            {
                var map = new PilotObjectMap(_objectsRepository);

                IEnumerable<object>? raw = context switch
                {
                    ObjectsViewContext c => c.SelectedObjects?.Cast<object>(),
                    DocumentFilesContext c => c.SelectedObjects?.Cast<object>(),
                    LinkedObjectsContext c => c.SelectedObjects?.Cast<object>(),
                    StorageContext c => c.SelectedObjects?.Cast<object>(),
                    TasksViewContext2 c => c.SelectedTasks?.Cast<object>(),
                    LinkedTasksContext2 c => c.SelectedTasks?.Cast<object>(),
                    DocumentAnnotationsListContext с => с.SelectedAnnotations?.Cast<object>(),

                    _ => null
                };

                // Выделенные ярлыки приходят отдельным свойством
                IEnumerable<object> shortcuts = context switch
                {
                    ObjectsViewContext c => c.Shortcuts?.Cast<object>(),
                    _ => null
                };

                // Материализуем один раз: ленивое перечисление через границу
                // AppDomain при повторном проходе может потерять элементы
                var rawList = raw?.ToList() ?? new List<object>();
                var shortcutList = shortcuts?.ToList() ?? new List<object>();
                var items = rawList.Concat(shortcutList).ToList();
                var shortcutCount = shortcutList.Count;

                var extra = context is ObjectsViewContext ctx ? $"; IsContext: {ctx.IsContext}" : string.Empty;

                if (items.Count > 0)
                {
                    _convertSelection = new ObjectSet(null);
                    _convertSelection.AddRange(items.Select(map.Wrap));
                    RaiseSelectionChanged(context.GetType().Name + extra, source, shortcutCount);
                }
                else
                {
                    RaiseSelectionCleared(context.GetType().Name + extra, source);
                }
            }
            catch (Exception e)
            {
                RaiseSelectionError(e);
            }
        }

        public void UpdateSelected(SignatureRequestsContext context, string source = null)
        {
            try
            {
                var map = new PilotObjectMap(_objectsRepository);

                var raw = context.SelectedSignatureRequests?.Cast<object>().ToList();

                if (raw?.Count > 0)
                {
                    _convertSelection = new ObjectSet(null);
                    _convertSelection.AddRange(raw.Select(map.Wrap));
                    RaiseSelectionChanged(context.GetType().Name, source, 0);
                }
                else
                {
                    RaiseSelectionCleared(context.GetType().Name, source);
                }
            }
            catch (Exception e)
            {
                RaiseSelectionError(e);
            }
        }

        public void UpdateSelected(ModelContext context, string source = null)
        {
            try
            {
                var map = new PilotObjectMap(_objectsRepository);

                var raw = context.SelectedElements?.Cast<object>().ToList();

                if (raw?.Count > 0)
                {
                    _convertSelection = new ObjectSet(null);
                    _convertSelection.AddRange(raw.Select(map.Wrap));
                    RaiseSelectionChanged(context.GetType().Name, source, 0);
                }
                else
                {
                    RaiseSelectionCleared(context.GetType().Name, source);
                }
            }
            catch (Exception e)
            {
                RaiseSelectionError(e);
            }
        }

        private void RaiseSelectionChanged(string contextName, string source, int shortcutCount)
        {
            // Одинаковые подряд выделения из одного источника не дублируем;
            // источник входит в ключ, чтобы событие от меню не гасилось событием от тулбара
            var key = source + "|" + contextName + "|" + string.Join(",", _convertSelection.Select(o => o.StringId));
            if (key == _lastRaisedSelection)
                return;
            _lastRaisedSelection = key;

            var names = _convertSelection.Select(SafeName).Take(5).ToList();
            var namesText = string.Join(", ", names);
            if (_convertSelection.Count > names.Count)
                namesText += $" … (+{_convertSelection.Count - names.Count})";

            var dataObjects = _convertSelection
                .Select(o => o.LookUpObject)
                .OfType<IDataObject>()
                .ToList();
            var dataObjectIds = dataObjects.Select(o => o.Id).ToList();
            var typeInfo = string.Join(", ", dataObjects
                .Select(o => SafeTypeTitle(o))
                .Where(t => !string.IsNullOrEmpty(t))
                .Distinct());

            var shortcutsText = shortcutCount > 0 ? $" (в т.ч. ярлыков: {shortcutCount})" : string.Empty;
            var sourceText = string.IsNullOrEmpty(source) ? string.Empty : $"Источник: {source}; ";

            _clientEventService.Raise(
                "SelectionChanged",
                $"{sourceText}Контекст: {contextName}; выделено: {_convertSelection.Count}{shortcutsText} — {namesText}",
                dataObjectIds,
                typeInfo);
        }

        private const string EmptySelectionKey = "<empty>";

        // Пустое выделение: сбрасываем дедупликацию (иначе повторное выделение
        // того же объекта после сброса не даст события) и сообщаем о сбросе один раз
        private void RaiseSelectionCleared(string contextName, string source)
        {
            if (_lastRaisedSelection == null || _lastRaisedSelection == EmptySelectionKey)
                return;
            _lastRaisedSelection = EmptySelectionKey;

            var sourceText = string.IsNullOrEmpty(source) ? string.Empty : $"Источник: {source}; ";
            _clientEventService.Raise(
                "SelectionChanged",
                $"{sourceText}Контекст: {contextName}; выделено: 0");
        }

        private static string SafeTypeTitle(IDataObject dataObject)
        {
            try
            {
                return dataObject.Type?.Title ?? dataObject.Type?.Name;
            }
            catch
            {
                return null;
            }
        }

        private static string SafeName(PilotObjectHelper helper)
        {
            try
            {
                return helper.Name ?? helper.StringId ?? "?";
            }
            catch
            {
                return "?";
            }
        }

        private void RaiseSelectionError(Exception e)
        {
            try
            {
                _clientEventService.Raise("SelectionError", e.ToString());
            }
            catch
            {
                // перехват выделения не должен ломать построение меню Pilot
            }
        }
    }
}
