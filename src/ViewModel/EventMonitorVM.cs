using Ascon.Pilot.SDK;
using PilotLookUp.Commands;
using PilotLookUp.Core;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace PilotLookUp.ViewModel
{
    public class EventMonitorVM : IPage, INotifyPropertyChanged, IDisposable
    {
        private const int MaxEvents = 2000;
        private const string AllCategories = "Все";

        private readonly IObjectsRepository _objectsRepository;
        private readonly ITabService _tabService;
        private readonly IRepoService _repoService;
        private readonly IPageService _pageController;
        private readonly IClientEventService _clientEventService;
        private readonly Dispatcher _dispatcher;
        private readonly NotificationListener _listener;

        public EventMonitorVM(
            IObjectsRepository objectsRepository,
            ITabService tabService,
            IRepoService repoService,
            IPageService pageController,
            IClientEventService clientEventService)
        {
            _objectsRepository = objectsRepository;
            _tabService = tabService;
            _repoService = repoService;
            _pageController = pageController;
            _clientEventService = clientEventService;
            _dispatcher = Dispatcher.CurrentDispatcher;

            Events = new ObservableCollection<EventMonitorItemVM>();
            EventsView = CollectionViewSource.GetDefaultView(Events);
            EventsView.Filter = FilterEvent;

            _listener = new NotificationListener(_objectsRepository);
            _listener.NotificationReceived += OnNotificationReceived;
            _clientEventService.EventRaised += OnClientEventRaised;
        }

        public PagesName GetName() => PagesName.EventMonitor;

        public ObservableCollection<EventMonitorItemVM> Events { get; }
        public ICollectionView EventsView { get; }

        public IReadOnlyList<string> Categories { get; } = new[] { AllCategories, "Objects", "Tasks", "Storage", "Client", "Other" };

        private string _selectedCategory = AllCategories;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                EventsView.Refresh();
            }
        }

        private string _textFilter;
        public string TextFilter
        {
            get => _textFilter;
            set
            {
                _textFilter = value;
                OnPropertyChanged();
                EventsView.Refresh();
            }
        }

        private EventMonitorItemVM _selectedEvent;
        public EventMonitorItemVM SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                _selectedEvent = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DetailVisibility));
            }
        }

        public System.Windows.Visibility DetailVisibility =>
            SelectedEvent == null ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;

        public string EventsCounter => $"Событий: {Events.Count}";

        public ICommand ClearCommand => new RelayCommand<object>(_ =>
        {
            Events.Clear();
            SelectedEvent = null;
            OnPropertyChanged(nameof(EventsCounter));
        });

        public ICommand GoToCommand => new RelayCommand<object>(_ => GoToObject());
        public ICommand OpenLookUpCommand => new RelayCommand<object>(_ => OpenLookUp());

        private async void GoToObject()
        {
            // Pilot умеет показать один объект — ведем к первому из события
            var navigateIds = SelectedEvent?.NavigateIds;
            if (navigateIds == null || navigateIds.Count == 0)
                return;

            var dataObject = await new ObjectLoader(_objectsRepository).LoadWithTimeout(navigateIds[0]);
            if (dataObject != null)
                _tabService.GoTo(dataObject);
        }

        private async void OpenLookUp()
        {
            var navigateIds = SelectedEvent?.NavigateIds;
            if (navigateIds == null || navigateIds.Count == 0)
                return;

            var objectSet = await _repoService.GetWrapedObjs(navigateIds);
            _pageController.CreatePage(PagesName.LookUpPage, objectSet);
        }

        // Вызывается из фонового потока SDK
        private void OnNotificationReceived(NotificationRecord record)
        {
            _dispatcher.BeginInvoke((Action)(() => AddEvent(new EventMonitorItemVM(record, _objectsRepository))));
        }

        private void OnClientEventRaised(ClientEventRecord record)
        {
            _dispatcher.BeginInvoke((Action)(() => AddEvent(new EventMonitorItemVM(record, _objectsRepository))));
        }

        private void AddEvent(EventMonitorItemVM item)
        {
            Events.Add(item);
            if (Events.Count > MaxEvents)
                Events.RemoveAt(0);
            OnPropertyChanged(nameof(EventsCounter));
        }

        private bool FilterEvent(object item)
        {
            if (!(item is EventMonitorItemVM eventItem))
                return false;

            if (SelectedCategory != AllCategories && eventItem.Category != SelectedCategory)
                return false;

            if (string.IsNullOrWhiteSpace(TextFilter))
                return true;

            return Contains(eventItem.Kind)
                || Contains(eventItem.Title)
                || Contains(eventItem.UserName)
                || Contains(eventItem.TypeTitle)
                || Contains(eventItem.Message)
                || Contains(eventItem.NotificationName);

            bool Contains(string source) =>
                !string.IsNullOrEmpty(source)
                && source.IndexOf(TextFilter, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public void Dispose()
        {
            _listener.NotificationReceived -= OnNotificationReceived;
            _listener.Dispose();
            _clientEventService.EventRaised -= OnClientEventRaised;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
