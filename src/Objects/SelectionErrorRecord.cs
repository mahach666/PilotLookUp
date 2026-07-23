using System;

namespace PilotLookUp.Objects
{
    // Ошибка при обработке выделения (перехват не должен ломать меню Pilot)
    public sealed class SelectionErrorRecord
    {
        public SelectionErrorRecord(string error)
        {
            Error = error;
            DateTime = DateTime.Now;
        }

        public string Error { get; }
        public DateTime DateTime { get; }
    }
}
