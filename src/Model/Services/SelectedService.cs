using Ascon.Pilot.Bim.SDK;
using Ascon.Pilot.Bim.SDK.ModelStorage;
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
        // Ограничение на глубокий резолв элементов модели (защита от заморозки на больших выделениях)
        private const int MaxBimElementsToLoad = 50;

        private IObjectsRepository _objectsRepository;
        private IClientEventService _clientEventService;
        private IBimModelService _bimModelService;
        private ObjectSet _convertSelection;
        private string _lastRaisedSelection;

        public SelectedService(
            IObjectsRepository objectsRepository,
            IClientEventService clientEventService,
            IBimModelService bimModelService)
        {
            _objectsRepository = objectsRepository;
            _clientEventService = clientEventService;
            _bimModelService = bimModelService;
        }

        public ObjectSet Selected => _convertSelection;

        public void UpdateSelected(MarshalByRefObject context, string source = null)
        {
            try
            {
                var map = new PilotObjectMap(_objectsRepository);

                IEnumerable<object> raw = context switch
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

                bool? isContext = context is ObjectsViewContext ctx ? ctx.IsContext : (bool?)null;

                if (items.Count > 0)
                {
                    _convertSelection = new ObjectSet(null);
                    _convertSelection.AddRange(items.Select(map.Wrap));
                    RaiseSelectionChanged(context.GetType().Name, source, isContext, shortcutList.Count);
                }
                else
                {
                    RaiseSelectionCleared(context.GetType().Name, source, isContext);
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
                    RaiseSelectionChanged(context.GetType().Name, source, null, 0);
                }
                else
                {
                    RaiseSelectionCleared(context.GetType().Name, source, null);
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

                var ids = context.SelectedElements?.ToList();

                if (ids?.Count > 0)
                {
                    _convertSelection = new ObjectSet(null);
                    var loaded = 0;

                    foreach (var id in ids)
                    {
                        // Элемент + свойства грузятся в момент выделения (из хелперов
                        // хранилище недоступно); сверх лимита - легкий id
                        var view = loaded < MaxBimElementsToLoad
                            ? _bimModelService.TryLoadElementView(id)
                            : null;

                        if (view != null)
                        {
                            _convertSelection.Add(map.Wrap(view));
                            loaded++;
                        }
                        else
                        {
                            _convertSelection.Add(map.Wrap(id));
                        }
                    }

                    RaiseSelectionChanged(context.GetType().Name, source, null, 0);
                }
                else
                {
                    RaiseSelectionCleared(context.GetType().Name, source, null);
                }
            }
            catch (Exception e)
            {
                RaiseSelectionError(e);
            }
        }

        private void RaiseSelectionChanged(string contextName, string source, bool? isContext, int shortcutsCount)
        {
            // Одинаковые подряд выделения из одного источника не дублируем;
            // источник входит в ключ, чтобы событие от меню не гасилось событием от тулбара
            var key = source + "|" + contextName + "|" + string.Join(",", _convertSelection.Select(o => o.StringId));
            if (key == _lastRaisedSelection)
                return;
            _lastRaisedSelection = key;

            var dataObjectIds = _convertSelection
                .Select(o => o.LookUpObject)
                .OfType<IDataObject>()
                .Select(o => o.Id)
                .ToList();

            _clientEventService.Raise(new SelectionChangedRecord(
                source,
                contextName,
                isContext,
                dataObjectIds,
                shortcutsCount,
                _convertSelection.Count));
        }

        private const string EmptySelectionKey = "<empty>";

        // Пустое выделение: сбрасываем дедупликацию (иначе повторное выделение
        // того же объекта после сброса не даст события) и сообщаем о сбросе один раз
        private void RaiseSelectionCleared(string contextName, string source, bool? isContext)
        {
            if (_lastRaisedSelection == null || _lastRaisedSelection == EmptySelectionKey)
                return;
            _lastRaisedSelection = EmptySelectionKey;

            _clientEventService.Raise(new SelectionChangedRecord(
                source,
                contextName,
                isContext,
                Array.Empty<Guid>(),
                0,
                0));
        }

        private void RaiseSelectionError(Exception e)
        {
            try
            {
                _clientEventService.Raise(new SelectionErrorRecord(e.ToString()));
            }
            catch
            {
                // перехват выделения не должен ломать построение меню Pilot
            }
        }
    }
}
