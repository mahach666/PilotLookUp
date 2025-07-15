namespace PilotLookUp.Infrastructure
{
    public class SimpleLogger : ILogger
    {
        public void Trace(string message)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[TRACE] {message}");
#endif
        }
        public void Debug(string message)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[DEBUG] {message}");
#endif
        }
        public void Info(string message)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[INFO] {message}");
#endif
        }
        public void Warn(string message)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[WARN] {message}");
#endif
        }
        public void Error(string message)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[ERROR] {message}");
#endif
        }
    }
} 