using Ascon.Pilot.SDK;
using PilotLookUp.Enums;
using PilotLookUp.Model.AdvancedSearch;
using PilotTestHost.Sdk;
using System;
using System.Linq;

namespace PilotLookUp.Tests
{
    // Справочники значений атрибутов: состояния своей машины, ФИО у оргединиц,
    // тип атрибута в подписи (в базе есть одноименные атрибуты разных типов)
    public class AttributeLookupTests
    {
        private static System.Collections.Generic.IReadOnlyList<FieldDescriptor> AttributeDetails(ITestContext ctx) =>
            new FieldCatalog(ctx.Repository).GetCategories().First(c => c.DisplayName == "Атрибут").Details;

        [PilotFact]
        public void Attribute_Titles_Show_Type(ITestContext ctx)
        {
            var details = AttributeDetails(ctx).Skip(4).ToList(); // первые 4 - пункты «Любой ...»

            Check.That(details.All(d => d.DisplayName.Contains(" · ")),
                "у каждого атрибута в подписи должен быть тип");
            Check.That(details.Any(d => d.DisplayName.EndsWith("состояние")), "нет атрибутов-состояний");
            Check.That(details.Any(d => d.DisplayName.EndsWith("строка")), "нет строковых атрибутов");
        }

        [PilotFact]
        public void Same_Named_Attributes_Of_Different_Types_Are_Distinguishable(ITestContext ctx)
        {
            // В базе есть grif и как строка, и как состояние - подписи не должны совпадать
            var names = AttributeDetails(ctx).Select(d => d.DisplayName).ToList();

            Check.Equal(names.Count, names.Distinct().Count(), "подписи атрибутов должны быть уникальны");
        }

        [PilotFact]
        public void Attribute_Title_Is_The_Most_Common_One(ITestContext ctx)
        {
            var all = ctx.Repository.GetTypes()
                .SelectMany(t => t.Attributes.Where(a => !a.IsService))
                .ToList();

            // Только имена с одним типом данных: иначе по имени нельзя однозначно
            // сопоставить дескриптор (name бывает и строкой, и справочником)
            var singleTypeNames = all.GroupBy(a => a.Name)
                .Where(g => g.Select(a => a.Type).Distinct().Count() == 1)
                .Select(g => g.Key)
                .ToHashSet();

            // Один атрибут в разных типах может называться по-разному; берем частый
            var definitions = all
                .Where(a => singleTypeNames.Contains(a.Name))
                .GroupBy(a => a.Name)
                .Where(g => g.Select(a => a.Title).Distinct().Count() > 1)
                .ToList();

            Check.That(definitions.Count > 0, "в базе должны быть атрибуты с разными заголовками");
            ctx.Logger.Info($"Проверяется атрибутов с разными заголовками: {definitions.Count}");

            var details = AttributeDetails(ctx);

            foreach (var group in definitions.Take(20))
            {
                var name = group.First().Name;
                var byCount = group.GroupBy(a => a.Title).OrderByDescending(g => g.Count()).ToList();

                // Заголовков с максимальной частотой может быть несколько
                // (у code два заголовка ровно по 507 типов) - подходит любой из них
                var topCount = byCount[0].Count();
                var mostCommon = byCount.Where(g => g.Count() == topCount).Select(g => g.Key).ToList();
                var rare = byCount[byCount.Count - 1].Key;

                var descriptor = details.FirstOrDefault(d => d.DisplayName.Contains($"({name}) · "));
                if (descriptor == null)
                    continue;

                Check.That(mostCommon.Any(t => descriptor.DisplayName.StartsWith(t + " (")),
                    $"'{name}': ожидался частый заголовок ({topCount} типов) из [{string.Join(", ", mostCommon)}], получено '{descriptor.DisplayName}'");

                // Редкий заголовок остается находимым через фильтр выпадашки
                Check.Contains(descriptor.SearchText, rare, $"'{name}': поиск по редкому заголовку");
            }
        }

        [PilotFact]
        public void UserState_Attribute_Offers_States_Of_All_Its_Machines(ITestContext ctx)
        {
            var attribute = ctx.Repository.GetTypes()
                .SelectMany(t => t.Attributes)
                .Where(a => !a.IsService && a.Type == AttributeType.UserState)
                .FirstOrDefault(a => Guid.TryParse(a.Configuration, out var id)
                    && ctx.Repository.GetUserStateMachines().Any(m => m.Id == id));

            Check.NotNull(attribute, "в базе должен быть атрибут-состояние с машиной состояний");

            // Все машины, которыми этот атрибут настроен в разных типах
            var machineIds = ctx.Repository.GetTypes()
                .SelectMany(t => t.Attributes)
                .Where(a => a.Name == attribute.Name && a.Type == AttributeType.UserState)
                .Select(a => Guid.TryParse(a.Configuration, out var id) ? id : Guid.Empty)
                .Where(id => id != Guid.Empty)
                .Distinct()
                .ToList();

            var expected = ctx.Repository.GetUserStateMachines()
                .Where(m => machineIds.Contains(m.Id))
                .SelectMany(m => m.StateTransitions.Keys)
                .Distinct()
                .ToHashSet();

            var descriptor = AttributeDetails(ctx)
                .First(d => d.DisplayName.Contains($"({attribute.Name}) · ") && d.DisplayName.EndsWith("состояние"));

            Check.Equal(ValueEditorKind.Lookup, descriptor.EditorKind, "редактор состояния - выбор из списка");

            var items = descriptor.LookupItemsProvider().ToList();
            Check.That(items.Count > 0, "список состояний пуст");

            Check.That(items.All(i => expected.Contains((Guid)i.Key)),
                "предлагаются состояния не из машин этого атрибута");
            Check.Equal(expected.Count, items.Count, "должны предлагаться состояния всех машин атрибута");

            var allStates = ctx.Repository.GetUserStates().Count();
            Check.That(items.Count < allStates,
                $"должны предлагаться только состояния атрибута ({items.Count}), а не все состояния базы ({allStates})");

            ctx.Logger.Info($"{attribute.Name}: {items.Count} состояний из {allStates} (машин: {machineIds.Count}): {string.Join(", ", items.Select(i => i.Title))}");
        }

        [PilotFact]
        public void Attribute_With_Several_State_Machines_Merges_Their_States(ITestContext ctx)
        {
            // В базе есть атрибуты, настроенные разными машинами в разных типах
            // (status_doc - три машины): искать нужно по любому допустимому состоянию
            var group = ctx.Repository.GetTypes()
                .SelectMany(t => t.Attributes.Where(a => !a.IsService && a.Type == AttributeType.UserState))
                .GroupBy(a => a.Name)
                .FirstOrDefault(g => g.Select(a => a.Configuration).Distinct().Count() > 1);

            Check.NotNull(group, "в базе должен быть атрибут с несколькими машинами состояний");

            var machines = ctx.Repository.GetUserStateMachines()
                .Where(m => group.Any(a => a.Configuration == m.Id.ToString()))
                .ToList();
            Check.That(machines.Count > 1, "должно найтись несколько машин");

            var descriptor = AttributeDetails(ctx)
                .First(d => d.DisplayName.Contains($"({group.Key}) · ") && d.DisplayName.EndsWith("состояние"));
            var items = descriptor.LookupItemsProvider().Select(i => (Guid)i.Key).ToHashSet();

            foreach (var machine in machines)
            {
                var known = machine.StateTransitions.Keys
                    .Where(k => ctx.Repository.GetUserStates().Any(s => s.Id == k))
                    .ToList();

                Check.That(known.All(items.Contains),
                    $"состояния машины '{machine.Title}' не попали в список атрибута {group.Key}");
            }

            ctx.Logger.Info($"{group.Key}: {items.Count} состояний из {machines.Count} машин ({string.Join(", ", machines.Select(m => m.Title))})");
        }

        [PilotFact]
        public void OrgUnit_Lookup_Allows_Search_By_Person_Name(ITestContext ctx)
        {
            var person = ctx.Repository.GetPeople().FirstOrDefault(p => p.Positions.Any());
            Check.NotNull(person, "в базе должен быть человек с должностью");

            var descriptor = AttributeDetails(ctx).FirstOrDefault(d => d.DisplayName.EndsWith("оргединица"));
            Check.NotNull(descriptor, "в базе должен быть атрибут-оргединица");

            var items = descriptor.LookupItemsProvider().ToList();

            var matched = items.Where(i => i.Title.IndexOf(person.DisplayName, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList();
            Check.That(matched.Count > 0,
                $"по ФИО '{person.DisplayName}' ничего не найдено; примеры: {string.Join(" | ", items.Take(5).Select(i => i.Title))}");

            // Ключ остается id оргединицы (должности), а не человека
            var positionIds = person.Positions.Select(p => p.Position).ToHashSet();
            Check.That(matched.Any(i => positionIds.Contains((int)i.Key)),
                "подпись с ФИО должна принадлежать должности этого человека");

            ctx.Logger.Info($"{person.DisplayName}: {string.Join(" | ", matched.Select(i => i.Title))}");
        }
    }
}
