using Ascon.Pilot.Bim.SDK.Model;
using Ascon.Pilot.Bim.SDK.ModelStorage;
using Ascon.Pilot.Bim.SDK.ModelViewer;
using Ascon.Pilot.SDK;
using PilotLookUp.Extensions;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotLookUp.Model.Services
{
    // Следит за жизненным циклом моделей Pilot-BIM: подписывается на события
    // каждого открытого вьюера и транслирует их в ленту событий (IClientEventService)
    public class BimModelService : IBimModelService, IDisposable
    {
        private readonly IModelManager _modelManager;
        private readonly IModelStorageProvider _modelStorageProvider;
        private readonly IClientEventService _clientEventService;
        private readonly IObjectsRepository _objectsRepository;

        private readonly object _sync = new object();
        private readonly Dictionary<Guid, IModelViewer> _viewers = new Dictionary<Guid, IModelViewer>();

        // Кэш "часть модели -> id, по которому нашлось хранилище":
        // избавляет от повторного поиска по БД для каждого элемента выделения
        private readonly ConcurrentDictionary<Guid, Guid> _partToStorageId = new ConcurrentDictionary<Guid, Guid>();

        public BimModelService(
            IModelManager modelManager,
            IModelStorageProvider modelStorageProvider,
            IClientEventService clientEventService,
            IObjectsRepository objectsRepository)
        {
            _modelManager = modelManager;
            _modelStorageProvider = modelStorageProvider;
            _clientEventService = clientEventService;
            _objectsRepository = objectsRepository;

            if (_modelManager == null)
                return;

            _modelManager.ModelOpened += OnModelOpened;
            _modelManager.ModelLoaded += OnModelLoaded;
            _modelManager.ModelClosed += OnModelClosed;
        }

        public bool IsAvailable => _modelManager != null;

        public IModelStorage GetStorage(Guid modelId)
        {
            try
            {
                return _modelStorageProvider?.GetStorage(modelId);
            }
            catch
            {
                return null;
            }
        }

        public IModelViewer TryGetViewer(Guid tabId)
        {
            lock (_sync)
            {
                return _viewers.TryGetValue(tabId, out var viewer) ? viewer : null;
            }
        }

        public BimElementView TryLoadElementView(Ascon.Pilot.Bim.SDK.IModelElementId id)
        {
            if (id == null || !IsAvailable)
                return null;

            foreach (var viewer in ViewersSnapshot())
            {
                var view = TryLoad(GetStorage(viewer.ModelId), id, viewer.ModelVersion);
                if (view != null)
                    return view;
            }

            // Вьюеры не дали результата - ищем модель через БД по части
            foreach (var storage in FindStoragesByPart(id.ModelPartId))
            {
                var view = TryLoad(storage, id, LatestVersion(storage));
                if (view != null)
                    return view;
            }

            return null;
        }

        // Вариант для голых Guid'ов: вместо null возвращает диагностику,
        // чтобы причина отказа была видна при снупе
        public BimElementView TryLoadElementView(Guid elementId, Guid modelPartId)
        {
            if (elementId == Guid.Empty)
                return null;

            var stub = new ModelElementIdStub(elementId, modelPartId);

            if (!IsAvailable)
                return new BimElementView(stub, null, null,
                    "BIM-сервисы клиента недоступны (IPilotServiceProvider не предоставил IModelManager)");

            var view = TryLoadElementView(stub);
            if (view != null)
                return view;

            var viewerCount = ViewersSnapshot().Count;
            var reason = viewerCount == 0
                ? "Нет отслеживаемых вьюеров (событие ModelOpened не приходило), и модель не нашлась по части через БД"
                : $"Элемент не найден в загруженных моделях (отслеживается вьюеров: {viewerCount})";
            return new BimElementView(stub, null, null, reason);
        }

        private List<IModelViewer> ViewersSnapshot()
        {
            lock (_sync)
            {
                return _viewers.Values.ToList();
            }
        }

        private static BimElementView TryLoad(IModelStorage storage, Ascon.Pilot.Bim.SDK.IModelElementId id, DateTime version)
        {
            try
            {
                if (storage == null || !storage.IsLoaded)
                    return null;

                var element = storage.LoadElement(id, version);
                if (element == null)
                    return null;

                var propertySets = storage.LoadElementProperties(id, version)?.ToList()
                    ?? new List<Ascon.Pilot.Bim.SDK.IPropertySet>();
                return new BimElementView(id, element, propertySets, null);
            }
            catch
            {
                // Модель могла выгрузиться - пробуем другие источники
                return null;
            }
        }

        private static DateTime LatestVersion(IModelStorage storage)
        {
            try
            {
                return storage.GetVersions()?.OrderBy(v => v).LastOrDefault() ?? default;
            }
            catch
            {
                return default;
            }
        }

        // Часть модели - документ Pilot; модель - один из его родителей.
        // Позволяет найти хранилище, даже если вьюеры не отслеживаются
        private IEnumerable<IModelStorage> FindStoragesByPart(Guid modelPartId)
        {
            var result = new List<IModelStorage>();
            if (_modelStorageProvider == null || modelPartId == Guid.Empty)
                return result;

            try
            {
                // Сначала кэш: путь уже находили для этой части
                if (_partToStorageId.TryGetValue(modelPartId, out var cachedId))
                {
                    var cachedStorage = GetStorage(cachedId);
                    if (cachedStorage != null && cachedStorage.IsLoaded)
                    {
                        result.Add(cachedStorage);
                        return result;
                    }
                }

                var candidateIds = CollectPartParentIds(modelPartId);

                foreach (var candidateId in candidateIds)
                {
                    var storage = GetStorage(candidateId);
                    if (storage != null && storage.IsLoaded)
                    {
                        _partToStorageId[modelPartId] = candidateId;
                        result.Add(storage);
                    }
                }
            }
            catch
            {
                // Поиск по БД - вспомогательный путь, сбои не критичны
            }

            return result;
        }

        // Ожидание строго через пул потоков и с жестким потолком: блокировать
        // вызывающий поток навсегда нельзя - SDK может маршалить свои вызовы
        // в UI-поток, и бесконечное ожидание превращается в дедлок клиента
        private List<Guid> CollectPartParentIds(Guid modelPartId)
        {
            var fallback = new List<Guid> { modelPartId };

            var task = Task.Run(async () =>
            {
                var ids = new List<Guid> { modelPartId };

                var current = await _objectsRepository.GetObjectWithTimeout(modelPartId);
                for (var i = 0; i < 3 && current != null && current.ParentId != default; i++)
                {
                    ids.Add(current.ParentId);
                    current = await _objectsRepository.GetObjectWithTimeout(current.ParentId);
                }

                return ids;
            });

            try
            {
                return task.Wait(2000) ? task.Result : fallback;
            }
            catch
            {
                return fallback;
            }
        }

        private void OnModelOpened(object sender, ModelEventArgs e)
        {
            Raise(() =>
            {
                TrackViewer(e.Viewer);
                return LifecycleRecord(BimModelStage.Opened, e.Viewer);
            });
        }

        private void OnModelLoaded(object sender, ModelEventArgs e)
        {
            Raise(() =>
            {
                // ModelLoaded может прийти для вьюера, которого еще нет в списке
                TrackViewer(e.Viewer);
                return LifecycleRecord(BimModelStage.Loaded, e.Viewer);
            });
        }

        private void OnModelClosed(object sender, ModelEventArgs e)
        {
            Raise(() =>
            {
                UntrackViewer(e.Viewer);
                return LifecycleRecord(BimModelStage.Closed, e.Viewer);
            });
        }

        private void OnViewerDisposed(object sender, EventArgs e)
        {
            Raise(() =>
            {
                var viewer = sender as IModelViewer;
                UntrackViewer(viewer);
                return LifecycleRecord(BimModelStage.Disposed, viewer);
            });
        }

        private void TrackViewer(IModelViewer viewer)
        {
            if (viewer == null)
                return;

            lock (_sync)
            {
                if (_viewers.ContainsKey(viewer.TabId))
                    return;
                _viewers.Add(viewer.TabId, viewer);
            }

            viewer.SelectionChanged += OnViewerSelectionChanged;
            viewer.ModelVersionAdded += OnModelVersionAdded;
            viewer.ModelVersionChanged += OnModelVersionChanged;
            viewer.ModelPartLoadingRequested += OnModelPartLoading;
            viewer.ModelPartUnloadingRequested += OnModelPartUnloading;
            viewer.TreeNodeExpanded += OnTreeNodeExpanded;
            viewer.TreeNodeCollapsed += OnTreeNodeCollapsed;
            viewer.Disposed += OnViewerDisposed;
        }

        private void UntrackViewer(IModelViewer viewer)
        {
            if (viewer == null)
                return;

            lock (_sync)
            {
                // Закрытие и Disposed могут прийти оба - отписка идемпотентна
                if (!_viewers.Remove(viewer.TabId))
                    return;
            }

            viewer.SelectionChanged -= OnViewerSelectionChanged;
            viewer.ModelVersionAdded -= OnModelVersionAdded;
            viewer.ModelVersionChanged -= OnModelVersionChanged;
            viewer.ModelPartLoadingRequested -= OnModelPartLoading;
            viewer.ModelPartUnloadingRequested -= OnModelPartUnloading;
            viewer.TreeNodeExpanded -= OnTreeNodeExpanded;
            viewer.TreeNodeCollapsed -= OnTreeNodeCollapsed;
            viewer.Disposed -= OnViewerDisposed;
        }

        private void OnViewerSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Raise(() =>
            {
                var viewer = (IModelViewer)sender;
                // Материализуем сразу: ленивые последовательности из аргументов событий не храним
                var ids = e.SelectedIds?.ToList() ?? new List<Ascon.Pilot.Bim.SDK.IModelElementId>();
                return new BimSelectionChangedRecord(viewer.ModelId, viewer.TabId, ids);
            });
        }

        private void OnModelVersionAdded(object sender, ModelVersionEventArgs e)
        {
            Raise(() =>
            {
                var viewer = (IModelViewer)sender;
                return new BimModelVersionRecord(viewer.ModelId, viewer.TabId, e.Version, BimVersionKind.Added);
            });
        }

        private void OnModelVersionChanged(object sender, ModelVersionEventArgs e)
        {
            Raise(() =>
            {
                var viewer = (IModelViewer)sender;
                return new BimModelVersionRecord(viewer.ModelId, viewer.TabId, e.Version, BimVersionKind.Changed);
            });
        }

        private void OnModelPartLoading(object sender, ModelPartEventArgs e)
        {
            Raise(() =>
            {
                var viewer = (IModelViewer)sender;
                return new BimModelPartRecord(viewer.ModelId, viewer.TabId, e.Id, BimPartKind.Loading);
            });
        }

        private void OnModelPartUnloading(object sender, ModelPartEventArgs e)
        {
            Raise(() =>
            {
                var viewer = (IModelViewer)sender;
                return new BimModelPartRecord(viewer.ModelId, viewer.TabId, e.Id, BimPartKind.Unloading);
            });
        }

        private void OnTreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            Raise(() =>
            {
                var viewer = (IModelViewer)sender;
                return new BimTreeNodeRecord(viewer.ModelId, viewer.TabId, e.Id, BimTreeNodeKind.Expanded);
            });
        }

        private void OnTreeNodeCollapsed(object sender, TreeNodeEventArgs e)
        {
            Raise(() =>
            {
                var viewer = (IModelViewer)sender;
                return new BimTreeNodeRecord(viewer.ModelId, viewer.TabId, e.Id, BimTreeNodeKind.Collapsed);
            });
        }

        private static BimModelLifecycleRecord LifecycleRecord(BimModelStage stage, IModelViewer viewer)
        {
            if (viewer == null)
                return new BimModelLifecycleRecord(stage, Guid.Empty, Guid.Empty, default);
            return new BimModelLifecycleRecord(stage, viewer.ModelId, viewer.TabId, viewer.ModelVersion);
        }

        // Мониторинг не должен ломать вьюер: любые сбои обработчиков глушим
        private void Raise(Func<object> recordFactory)
        {
            try
            {
                _clientEventService.Raise(recordFactory());
            }
            catch
            {
            }
        }

        public void Dispose()
        {
            if (_modelManager == null)
                return;

            _modelManager.ModelOpened -= OnModelOpened;
            _modelManager.ModelLoaded -= OnModelLoaded;
            _modelManager.ModelClosed -= OnModelClosed;

            List<IModelViewer> viewers;
            lock (_sync)
            {
                viewers = _viewers.Values.ToList();
            }
            foreach (var viewer in viewers)
                UntrackViewer(viewer);
        }
    }
}
