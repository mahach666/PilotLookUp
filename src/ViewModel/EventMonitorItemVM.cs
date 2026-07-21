using Ascon.Pilot.SDK;
using PilotLookUp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.ViewModel
{
    public class EventMonitorItemVM
    {
        // Серверное уведомление
        public EventMonitorItemVM(NotificationRecord record, IObjectsRepository repository)
        {
            EventDateTime = record.DateTime.ToLocalTime();
            Kind = record.ChangeKind.ToString();
            Title = record.Title;
            UserName = ResolveUserName(record, repository);
            TypeTitle = ResolveTypeTitle(record, repository);
            Category = GetCategory(record.ChangeKind);
            NavigateIds = new[] { record.ObjectId };
            ObjectId = record.ObjectId.ToString();
            UserInfo = record.UserId.HasValue ? $"{UserName} ({record.UserId})" : UserName;
            TypeInfo = $"{TypeTitle} ({record.TypeId})";
            NotificationName = record.NotificationName;
            Message = record.Message;
            ChangesetId = record.ChangesetId.ToString();
        }

        // Клиентское событие (выделение, клик меню)
        public EventMonitorItemVM(ClientEventRecord record, IObjectsRepository repository)
        {
            EventDateTime = record.DateTime;
            Kind = record.Name;
            Title = record.Message;
            UserName = ResolveCurrentUserName(repository);
            TypeTitle = record.TypeInfo;
            Category = "Client";
            NavigateIds = record.ObjectIds;
            ObjectId = string.Join(Environment.NewLine, record.ObjectIds);
            UserInfo = UserName;
            TypeInfo = record.TypeInfo;
            NotificationName = record.Name;
            Message = record.Message;
            ChangesetId = string.Empty;
        }

        public DateTime EventDateTime { get; }
        public string Time => EventDateTime.ToString("HH:mm:ss");
        public string FullTime => EventDateTime.ToString("dd.MM.yyyy HH:mm:ss");
        public string Kind { get; }
        public string Title { get; }
        public string UserName { get; }
        public string TypeTitle { get; }
        public string Category { get; }

        // Id объектов для навигации (пусто — переход недоступен)
        public IReadOnlyList<Guid> NavigateIds { get; }

        public string ObjectId { get; }
        public string UserInfo { get; }
        public string TypeInfo { get; }
        public string NotificationName { get; }
        public string Message { get; }
        public string ChangesetId { get; }

        private static string ResolveUserName(NotificationRecord record, IObjectsRepository repository)
        {
            if (!record.UserId.HasValue)
                return string.Empty;

            try
            {
                return repository.GetPerson(record.UserId.Value)?.DisplayName ?? record.UserId.ToString();
            }
            catch
            {
                return record.UserId.ToString();
            }
        }

        private static string ResolveTypeTitle(NotificationRecord record, IObjectsRepository repository)
        {
            try
            {
                return repository.GetType(record.TypeId)?.Title ?? record.TypeId.ToString();
            }
            catch
            {
                return record.TypeId.ToString();
            }
        }

        private static string ResolveCurrentUserName(IObjectsRepository repository)
        {
            try
            {
                return repository.GetCurrentPerson()?.DisplayName ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

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
