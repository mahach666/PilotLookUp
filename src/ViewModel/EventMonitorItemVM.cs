using Ascon.Pilot.SDK;
using PilotLookUp.Objects;
using System;

namespace PilotLookUp.ViewModel
{
    // Элемент ленты событий: запись оборачивается в PilotObjectHelper
    // и дальше снупается штатным механизмом (GetObjInfo), как в LookUp
    public class EventMonitorItemVM
    {
        public EventMonitorItemVM(NotificationRecord record, IObjectsRepository repository)
        {
            Helper = new PilotObjectMap(repository).Wrap(record);
            EventDateTime = record.DateTime.ToLocalTime();
            Kind = record.ChangeKind.ToString();
            Title = record.Title;
            Category = GetCategory(record.ChangeKind);
            SearchText = $"{Kind} {record.Title} {record.Message} {record.NotificationName}";
        }

        // Клиентские события (выделение, клики меню, ошибки перехвата)
        public EventMonitorItemVM(object clientRecord, IObjectsRepository repository)
        {
            Helper = new PilotObjectMap(repository).Wrap(clientRecord);
            Category = "Client";

            switch (clientRecord)
            {
                case SelectionChangedRecord selection:
                    EventDateTime = selection.DateTime;
                    Kind = "SelectionChanged";
                    Title = $"{selection.ContextName}: выделено {selection.SelectedCount}";
                    break;
                case MenuItemClickRecord menuClick:
                    EventDateTime = menuClick.DateTime;
                    Kind = "MenuItemClick";
                    Title = menuClick.ItemName;
                    break;
                case SelectionErrorRecord error:
                    EventDateTime = error.DateTime;
                    Kind = "SelectionError";
                    Title = error.Error;
                    break;
                default:
                    EventDateTime = DateTime.Now;
                    Kind = clientRecord.GetType().Name;
                    Title = clientRecord.ToString();
                    break;
            }

            SearchText = $"{Kind} {Title}";
        }

        public PilotObjectHelper Helper { get; }
        public DateTime EventDateTime { get; }
        public string Time => EventDateTime.ToString("HH:mm:ss");
        public string Kind { get; }
        public string Title { get; }
        public string Category { get; }

        // Склеенный текст для фильтра
        public string SearchText { get; }

        public static string GetCategory(NotificationKind kind)
        {
            var name = kind.ToString();
            if (name.StartsWith("Task"))
                return "Tasks";
            if (name.StartsWith("Storage"))
                return "Storage";
            if (name.StartsWith("Object"))
                return "Objects";
            return "Other";
        }
    }
}
