using System;
using System.Collections.Generic;

namespace PilotLookUp.Model.AdvancedSearch
{
    // Быстрые диапазоны дат (по мотивам ISmartFolderQueryBuilder.WithCreated*).
    // Диапазоны локальные; в UTC конвертирует FieldDescriptor при сборке терма
    public sealed class DatePreset
    {
        private readonly Func<(DateTime From, DateTime To)> _range;

        public DatePreset(string title, Func<(DateTime, DateTime)> range)
        {
            Title = title;
            _range = range;
        }

        public string Title { get; }

        // null - «Произвольно», даты вводятся вручную
        public (DateTime From, DateTime To)? GetRange() => _range?.Invoke();

        public override string ToString() => Title;

        public static IReadOnlyList<DatePreset> All { get; } = new List<DatePreset>
        {
            new DatePreset("Произвольно", null),
            new DatePreset("Сегодня", () => (DateTime.Today, DateTime.Today)),
            new DatePreset("Вчера", () => (DateTime.Today.AddDays(-1), DateTime.Today.AddDays(-1))),
            new DatePreset("Эта неделя", () => (StartOfWeek(DateTime.Today), DateTime.Today)),
            new DatePreset("Прошлая неделя", () =>
            {
                var start = StartOfWeek(DateTime.Today).AddDays(-7);
                return (start, start.AddDays(6));
            }),
            new DatePreset("Этот месяц", () =>
            {
                var today = DateTime.Today;
                return (new DateTime(today.Year, today.Month, 1), today);
            }),
            new DatePreset("Этот год", () =>
            {
                var today = DateTime.Today;
                return (new DateTime(today.Year, 1, 1), today);
            })
        };

        private static DateTime StartOfWeek(DateTime date)
        {
            var diff = ((int)date.DayOfWeek + 6) % 7; // понедельник - начало недели
            return date.AddDays(-diff);
        }
    }
}
