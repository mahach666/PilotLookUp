using PilotLookUp.Model.AdvancedSearch;
using PilotLookUp.Model.Services;
using PilotLookUp.Objects;
using PilotTestHost.Sdk;
using System;
using System.Linq;

namespace PilotLookUp.Tests
{
    // Утилиты: пресеты дат, кэш иконок, справочные элементы, роутинг меню
    public class UtilsTests
    {
        [PilotFact]
        public void DatePresets_Produce_Expected_Ranges()
        {
            var presets = DatePreset.All;
            Check.Equal(7, presets.Count, "семь пресетов");

            Check.That(presets[0].GetRange() == null, "«Произвольно» - без диапазона");

            var today = presets.First(p => p.Title == "Сегодня").GetRange().Value;
            Check.Equal(DateTime.Today, today.From, "сегодня: начало");
            Check.Equal(DateTime.Today, today.To, "сегодня: конец");

            var week = presets.First(p => p.Title == "Эта неделя").GetRange().Value;
            Check.Equal(DayOfWeek.Monday, week.From.DayOfWeek, "неделя начинается с понедельника");
            Check.That(week.From <= DateTime.Today && week.To == DateTime.Today, "неделя до сегодня");

            var lastWeek = presets.First(p => p.Title == "Прошлая неделя").GetRange().Value;
            Check.Equal(DayOfWeek.Monday, lastWeek.From.DayOfWeek, "прошлая неделя с понедельника");
            Check.Equal(6, (lastWeek.To - lastWeek.From).Days, "прошлая неделя - 7 дней");
            Check.That(lastWeek.To < week.From, "прошлая неделя раньше этой");

            var month = presets.First(p => p.Title == "Этот месяц").GetRange().Value;
            Check.Equal(1, month.From.Day, "месяц с первого числа");
        }

        [PilotFact]
        public void TypeIcons_Are_Cached_And_Frozen()
        {
            var first = TypeIcons.Get("guidIcon");
            var second = TypeIcons.Get("guidIcon");

            Check.NotNull(first, "иконка загружена из ресурсов");
            Check.That(ReferenceEquals(first, second), "повторный запрос отдает кэш");
            Check.That(first.IsFrozen, "иконка заморожена (безопасна между потоками)");
        }

        [PilotFact]
        public void Lookup_And_Choice_Items_Display_Title()
        {
            Check.Equal("Документ", new LookupItem(42, "Документ").ToString(), "LookupItem.ToString");
            Check.Equal("Не равно", new ChoiceItem(1, "Не равно").ToString(), "ChoiceItem.ToString");
        }

        [PilotFact]
        public void MenuCommandHandler_Raises_Click_Record_For_Any_Item()
        {
            var clientEvents = new ClientEventService();
            object raised = null;
            clientEvents.EventRaised += r => raised = r;

            // Фабрика и выделение не нужны: неизвестный пункт и пустое выделение их не трогают
            var handler = new MenuCommandHandler(null, new FakeEmptySelection(), clientEvents);

            handler.ItemClick("НеизвестныйПункт");
            Check.Equal("НеизвестныйПункт", (raised as MenuItemClickRecord)?.ItemName, "клик записан в ленту");

            handler.ItemClick("LookSelected");
            Check.Equal("LookSelected", (raised as MenuItemClickRecord)?.ItemName,
                "LookSelected с пустым выделением безопасен");
        }

        private sealed class FakeEmptySelection : PilotLookUp.Interfaces.ISelectedService
        {
            public ObjectSet Selected => null;
            public void UpdateSelected(MarshalByRefObject context, string source = null) { }
            public void UpdateSelected(Ascon.Pilot.SDK.SignatureRequestsContext context, string source = null) { }
            public void UpdateSelected(Ascon.Pilot.Bim.SDK.ModelTab.Menu.ModelContext context, string source = null) { }
        }
    }
}
