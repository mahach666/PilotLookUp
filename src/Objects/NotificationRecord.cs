using Ascon.Pilot.SDK;
using System;

namespace PilotLookUp.Objects
{
    // Снимок INotification: сам INotification - временный DTO,
    // поэтому все поля копируются сразу при получении
    public sealed class NotificationRecord
    {
        public NotificationRecord(INotification notification)
        {
            ObjectId = notification.ObjectId;
            Title = notification.Title;
            UserId = notification.UserId;
            DateTime = notification.DateTime;
            TypeId = notification.TypeId;
            ChangeKind = notification.ChangeKind;
            NotificationName = notification.NotificationName;
            Message = notification.Message;
            ChangesetId = notification.ChangesetId();
        }

        public Guid ObjectId { get; }
        public string Title { get; }
        public int? UserId { get; }
        public DateTime DateTime { get; }
        public int TypeId { get; }
        public NotificationKind ChangeKind { get; }
        public string NotificationName { get; }
        public string Message { get; }
        public long ChangesetId { get; }
    }
}
