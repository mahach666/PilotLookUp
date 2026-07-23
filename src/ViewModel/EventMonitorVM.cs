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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace PilotLookUp.ViewModel
{
    // Та же вкладка, что LookUp: слева лента событий, справа Info-грид (GetObjInfo)
    public class EventMonitorVM : IPage, INotifyPropertyChanged, IDisposable
    {
        private const int MaxEvents = 2000;
        private const string AllCategories = "Все";

        private readonly IObjectsRepository _objectsRepository;
        private readonly IRepoService _repoService;
        private readonly IViewFactory _viewFactory;
        private readonly IClientEventService _clientEventService;
        private readonly Dispatcher _dispatcher;
        private readonly NotificationListener _listener;

        public EventMonitorVM(
            IObjectsRepository objectsRepository,
            IRepoService repoService,
            IViewFactory viewFactory,
            IClientEventService clientEventService)
        {
            _objectsRepository = objectsRepository;
            _repoService = repoService;
            _viewFactory = viewFactory;
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
                OnPropertyChanged(nameof(PromptVisibility));
                EventsView.Refresh();
            }
        }

        public Visibility PromptVisibility => string.IsNullOrEmpty(_textFilter) ? Visibility.Visible : Visibility.Hidden;

        private EventMonitorItemVM _selectedEvent;
        public EventMonitorItemVM SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                if (_selectedEvent != value)
                {
                    _selectedEvent = value;
                    if (_selectedEvent != null)
                        UpdateInfo();
                    else
                        Info = null;
                    OnPropertyChanged();
                }
            }
        }

        private async void UpdateInfo()
        {
            var selected = _selectedEvent;
            try
            {
                var info = await Task.Run(() => _repoService.GetObjInfo(selected.Helper));

                // Выделение могло смениться, пока грузилась информация
                if (ReferenceEquals(selected, _selectedEvent))
                    Info = info;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private List<ObjectSet> _info;
        public List<ObjectSet> Info
        {
            get => _info;
            set
            {
                _info = value;
                OnPropertyChanged();
            }
        }

        private ObjectSet _dataGridSelected;
        public ObjectSet DataGridSelected
        {
            get => _dataGridSelected;
            set
            {
                _dataGridSelected = value;
                OnPropertyChanged();
            }
        }

        private ICommand _clearCommand;
        public ICommand ClearCommand => _clearCommand ??= new RelayCommand<object>(_ =>
        {
            Events.Clear();
            SelectedEvent = null;
        });

        private ICommand _selectedValueCommand;
        public ICommand SelectedValueCommand => _selectedValueCommand ??= new RelayCommand<object>(_ => _viewFactory.LookSelection(_dataGridSelected));

        private ICommand _copyCommand;
        public ICommand CopyCommand => _copyCommand ??= new RelayCommand<string>(CopyToClipboard);

        private void CopyToClipboard(string sender)
        {
            try
            {
                if (sender == "DataGridSelectName")
                    Clipboard.SetText(_dataGridSelected?.SenderMemberName ?? string.Empty);
                else if (sender == "DataGridSelectValue")
                    Clipboard.SetText(_dataGridSelected?.Description ?? string.Empty);
                else if (sender == "DataGridSelectLine")
                    Clipboard.SetText(_dataGridSelected?.SenderMemberName + "\t" + _dataGridSelected?.Description);
            }
            catch
            {
                MessageBox.Show("Упс, ничего не выбрано.");
            }
        }

        // Вызывается из фонового потока SDK
        private void OnNotificationReceived(NotificationRecord record)
        {
            _dispatcher.BeginInvoke((Action)(() => AddEvent(new EventMonitorItemVM(record, _objectsRepository))));
        }

        private void OnClientEventRaised(object record)
        {
            _dispatcher.BeginInvoke((Action)(() => AddEvent(new EventMonitorItemVM(record, _objectsRepository))));
        }

        private void AddEvent(EventMonitorItemVM item)
        {
            Events.Add(item);
            if (Events.Count > MaxEvents)
                Events.RemoveAt(0);
        }

        private bool FilterEvent(object item)
        {
            if (!(item is EventMonitorItemVM eventItem))
                return false;

            if (SelectedCategory != AllCategories && eventItem.Category != SelectedCategory)
                return false;

            if (string.IsNullOrWhiteSpace(TextFilter))
                return true;

            return eventItem.SearchText.IndexOf(TextFilter, StringComparison.OrdinalIgnoreCase) >= 0;
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
