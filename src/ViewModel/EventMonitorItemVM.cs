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
            Category = GetNotificationCategory(record.ChangeKind);
            SearchText = $"{Kind} {record.Title} {record.Message} {record.NotificationName}";
        }

        // Клиентские события (выделение, клики меню, ошибки перехвата) и BIM-события
        public EventMonitorItemVM(object clientRecord, IObjectsRepository repository)
        {
            Helper = new PilotObjectMap(repository).Wrap(clientRecord);
            var category = "Client";

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

                case BimModelLifecycleRecord lifecycle:
                    EventDateTime = lifecycle.DateTime;
                    Kind = "Model" + lifecycle.Stage;
                    Title = $"Модель {lifecycle.ModelId}";
                    category = "Bim";
                    break;
                case BimSelectionChangedRecord bimSelection:
                    EventDateTime = bimSelection.DateTime;
                    Kind = "BimSelectionChanged";
                    Title = $"Выделено {bimSelection.SelectedCount}";
                    category = "Bim";
                    break;
                case BimModelVersionRecord version:
                    EventDateTime = version.DateTime;
                    Kind = "ModelVersion" + version.Kind;
                    Title = version.Version.ToString("dd.MM.yyyy HH:mm:ss");
                    category = "Bim";
                    break;
                case BimModelPartRecord part:
                    EventDateTime = part.DateTime;
                    Kind = "ModelPart" + part.Kind;
                    Title = part.PartId.ToString();
                    category = "Bim";
                    break;
                case BimTreeNodeRecord treeNode:
                    EventDateTime = treeNode.DateTime;
                    Kind = "TreeNode" + treeNode.Kind;
                    Title = treeNode.Id?.ElementId.ToString();
                    category = "Bim";
                    break;

                default:
                    EventDateTime = DateTime.Now;
                    Kind = clientRecord.GetType().Name;
                    Title = clientRecord.ToString();
                    break;
            }

            Category = category;
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

        public static string GetNotificationCategory(NotificationKind kind)
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
