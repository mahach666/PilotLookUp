using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PilotLookUp.Tests
{
    // Ожидание на UI-потоке без его блокировки (SDK доставляет колбэки через UI)
    public static class TestPump
    {
        // Прокачать очередь сообщений до фонового приоритета (обработать BeginInvoke)
        public static void DoEvents()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(() => frame.Continue = false));
            Dispatcher.PushFrame(frame);
        }

        public static void UntilCompleted(Task task, TimeSpan timeout)
        {
            var frame = new DispatcherFrame();

            task.ContinueWith(_ => frame.Continue = false, TaskScheduler.Default);

            // Normal, не Background: фоновый приоритет голодает, если ожидаемая
            // задача непрерывно постит континуации в диспетчер
            var timer = new DispatcherTimer(DispatcherPriority.Normal) { Interval = timeout };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                frame.Continue = false;
            };
            timer.Start();

            Dispatcher.PushFrame(frame);
            timer.Stop();
        }

        public static T Await<T>(Task<T> task, int timeoutSeconds = 20)
        {
            UntilCompleted(task, TimeSpan.FromSeconds(timeoutSeconds));
            if (!task.IsCompleted)
                throw new AssertFailedException($"операция не завершилась за {timeoutSeconds} с");
            return task.GetAwaiter().GetResult();
        }
    }
}
