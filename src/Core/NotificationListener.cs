using Ascon.Pilot.SDK;
using PilotLookUp.Objects;
using System;
using System.Collections.Generic;

namespace PilotLookUp.Core
{
    public class NotificationListener : IObserver<INotification>, IDisposable
    {
        private readonly List<IDisposable> _subscriptions = new List<IDisposable>();

        public event Action<NotificationRecord> NotificationReceived;

        public NotificationListener(IObjectsRepository repository)
        {
            foreach (NotificationKind kind in Enum.GetValues(typeof(NotificationKind)))
                _subscriptions.Add(repository.SubscribeNotification(kind).Subscribe(this));
        }

        public void OnNext(INotification value)
        {
            // Копируем поля сразу: INotification живет ограниченное время
            NotificationReceived?.Invoke(new NotificationRecord(value));
        }

        // Ошибка одной подписки не должна останавливать остальные
        public void OnError(Exception error) { }

        public void OnCompleted() { }

        public void Dispose()
        {
            foreach (var subscription in _subscriptions)
                subscription.Dispose();
            _subscriptions.Clear();
            NotificationReceived = null;
        }
    }
}
