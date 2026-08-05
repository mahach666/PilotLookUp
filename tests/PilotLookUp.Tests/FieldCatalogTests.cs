using PilotLookUp.Enums;
using PilotLookUp.Model.AdvancedSearch;
using PilotTestHost.Sdk;
using System.Linq;

namespace PilotLookUp.Tests
{
    // Каталог категорий продвинутого поиска на живой базе
    public class FieldCatalogTests
    {
        [PilotFact]
        public void Categories_Have_Expected_Set(ITestContext ctx)
        {
            var categories = new FieldCatalog(ctx.Repository).GetCategories();
            var names = categories.Select(c => c.DisplayName).ToList();

            foreach (var expected in new[]
            {
                "Текст", "Тип", "Атрибут", "Автор", "Дата создания", "Дата изменения",
                "Состояние объекта", "Секретность", "Id объекта", "Системное поле"
            })
            {
                Check.That(names.Contains(expected), $"нет категории '{expected}'; есть: {string.Join(", ", names)}");
            }
        }

        [PilotFact]
        public void Attribute_Category_Starts_With_Any_Descriptors(ITestContext ctx)
        {
            var categories = new FieldCatalog(ctx.Repository).GetCategories();
            var attributes = categories.First(c => c.DisplayName == "Атрибут");

            Check.That(attributes.HasDetails, "у категории Атрибут должна быть детализация");

            var first4 = attributes.Details.Take(4).Select(d => d.DisplayName).ToList();
            Check.Contains(string.Join("; ", first4), "Любой строковый", "спец-пункт строковый");
            Check.Contains(string.Join("; ", first4), "Любой целочисленный", "спец-пункт целочисленный");
            Check.Contains(string.Join("; ", first4), "Любой дробный", "спец-пункт дробный");
            Check.Contains(string.Join("; ", first4), "Любой атрибут-дата", "спец-пункт дата");

            ctx.Logger.Info($"Всего детализаций атрибута: {attributes.Details.Count}");
        }

        [PilotFact]
        public void Direct_Categories_Have_No_Details_And_Detail_Categories_Have_Fields(ITestContext ctx)
        {
            var categories = new FieldCatalog(ctx.Repository).GetCategories();

            foreach (var category in categories)
            {
                if (category.HasDetails)
                {
                    Check.That(category.DirectField == null, $"'{category.DisplayName}': детализация и прямое поле одновременно");
                    Check.That(category.Details.Count > 0, $"'{category.DisplayName}': пустая детализация");
                }
                else
                {
                    Check.NotNull(category.DirectField, $"прямое поле '{category.DisplayName}'");
                }
            }
        }

        [PilotFact]
        public void Attribute_Editors_Match_Attribute_Kinds(ITestContext ctx)
        {
            var categories = new FieldCatalog(ctx.Repository).GetCategories();
            var details = categories.First(c => c.DisplayName == "Атрибут").Details;

            // На живой базе с 2900+ типами должны встретиться все основные виды редакторов
            var kinds = details.Select(d => d.EditorKind).Distinct().ToList();
            Check.That(kinds.Contains(ValueEditorKind.Text), "нет строковых атрибутов");
            Check.That(kinds.Contains(ValueEditorKind.DateRange), "нет атрибутов-дат");

            ctx.Logger.Info("Виды редакторов в атрибутах: " + string.Join(", ", kinds));
        }

        [PilotFact]
        public void System_Field_Details_Contain_Known_Fields(ITestContext ctx)
        {
            var categories = new FieldCatalog(ctx.Repository).GetCategories();
            var system = categories.First(c => c.DisplayName == "Системное поле");
            var names = string.Join("; ", system.Details.Select(d => d.DisplayName));

            Check.Contains(names, "Id родителя", "поле родителя");
            Check.Contains(names, "Размер всех файлов", "поле размера");
            Check.Contains(names, "Тег задачи", "тег задачи");
        }

        [PilotFact]
        public void Type_Lookup_Returns_Items(ITestContext ctx)
        {
            var categories = new FieldCatalog(ctx.Repository).GetCategories();
            var typeCategory = categories.First(c => c.DisplayName == "Тип");

            var items = typeCategory.DirectField.LookupItemsProvider().ToList();
            Check.That(items.Count > 0, "справочник типов пуст");
            ctx.Logger.Info($"Типов в справочнике: {items.Count}");
        }
    }
}
