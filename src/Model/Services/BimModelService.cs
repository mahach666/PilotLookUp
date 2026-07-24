using Ascon.Pilot.Bim.SDK.Model;
using Ascon.Pilot.Bim.SDK.ModelStorage;
using Ascon.Pilot.Bim.SDK.ModelViewer;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Model.Services
{
    // Следит за жизненным циклом моделей Pilot-BIM: подписывается на события
    // каждого открытого вьюера и транслирует их в ленту событий (IClientEventService)
    public class BimModelService : IBimModelService, IDisposable
    {
        private readonly IModelManager _modelManager;
        private readonly IModelStorageProvider _modelStorageProvider;
        private readonly IClientEventService _clientEventService;

        private readonly object _sync = new object();
        private readonly Dictionary<Guid, IModelViewer> _viewers = new Dictionary<Guid, IModelViewer>();

        public BimModelService(
            IModelManager modelManager,
            IModelStorageProvider modelStorageProvider,
            IClientEventService clientEventService)
        {
            _modelManager = modelManager;
            _modelStorageProvider = modelStorageProvider;
            _clientEventService = clientEventService;

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

            List<IModelViewer> viewers;
            lock (_sync)
            {
                viewers = _viewers.Values.ToList();
            }

            foreach (var viewer in viewers)
            {
                try
                {
                    var storage = GetStorage(viewer.ModelId);
                    if (storage == null || !storage.IsLoaded)
                        continue;

                    // Элемент принадлежит модели, если ее хранилище знает его часть
                    if (storage.GetModelPartsIds()?.Contains(id.ModelPartId) != true)
                        continue;

                    var element = storage.LoadElement(id, viewer.ModelVersion);
                    if (element == null)
                        continue;

                    var propertySets = storage.LoadElementProperties(id, viewer.ModelVersion)?.ToList()
                        ?? new List<Ascon.Pilot.Bim.SDK.IPropertySet>();
                    return new BimElementView(id, element, propertySets, null);
                }
                catch
                {
                    // Модель могла выгрузиться между итерациями - пробуем следующую
                }
            }

            return null;
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
