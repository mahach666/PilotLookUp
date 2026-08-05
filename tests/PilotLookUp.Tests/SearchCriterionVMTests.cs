using PilotLookUp.Enums;
using PilotLookUp.Model.AdvancedSearch;
using PilotLookUp.Tests;
using PilotLookUp.ViewModel;
using PilotLookUp.ViewModel.Editors;
using PilotTestHost.Sdk;
using System.Linq;

namespace PilotLookUp.Tests
{
    // Логика строки критерия: категории, детализация, операторы, редакторы
    public class SearchCriterionVMTests
    {
        private static SearchCriterionVM CreateRow(ITestContext ctx)
        {
            var categories = new FieldCatalog(ctx.Repository).GetCategories();
            return new SearchCriterionVM(categories, _ => { });
        }

        [PilotFact]
        public void New_Row_Has_Category_Operator_And_Editor_Preselected(ITestContext ctx)
        {
            var row = CreateRow(ctx);

            Check.NotNull(row.SelectedCategory, "категория по умолчанию");
            Check.That(row.OperatorOptions.Count > 0, "операторы должны быть заполнены");
            Check.NotNull(row.SelectedOperator, "оператор по умолчанию");
            Check.That(!(row.Editor is NoValueVM), "редактор по умолчанию не должен быть пустым (Текст/Равно)");
        }

        [PilotFact]
        public void Occur_Options_Are_And_Or_Only(ITestContext ctx)
        {
            var row = CreateRow(ctx);
            var titles = row.OccurOptions.Select(o => o.Title).ToList();

            Check.Equal(2, titles.Count, "количество сочетаний");
            Check.That(titles.Contains("И") && titles.Contains("ИЛИ"), "должны быть только И и ИЛИ");
        }

        [PilotFact]
        public void Attribute_Category_Shows_Details_And_Direct_Hides(ITestContext ctx)
        {
            var row = CreateRow(ctx);

            row.SelectedCategory = row.Categories.First(c => c.DisplayName == "Атрибут");
            Check.Equal(System.Windows.Visibility.Visible, row.DetailVisibility, "детализация видна для Атрибута");
            Check.NotNull(row.SelectedDetail, "первая детализация выбрана");

            row.SelectedCategory = row.Categories.First(c => c.DisplayName == "Текст");
            Check.Equal(System.Windows.Visibility.Collapsed, row.DetailVisibility, "детализация скрыта для Текста");
        }

        [PilotFact]
        public void NotExists_Operator_Gives_NoValue_Editor(ITestContext ctx)
        {
            var row = CreateRow(ctx);
            row.SelectedCategory = row.Categories.First(c => c.DisplayName == "Текст");

            row.SelectedOperator = row.OperatorOptions.First(o => (CriterionOperator)o.Value == CriterionOperator.NotExists);
            Check.That(row.Editor is NoValueVM, "Не заполнено не требует значения");
        }

        [PilotFact]
        public void Number_Range_Operator_Gives_Two_Field_Editor(ITestContext ctx)
        {
            var row = CreateRow(ctx);
            row.SelectedCategory = row.Categories.First(c => c.DisplayName == "Системное поле");
            row.SelectedDetail = row.DetailOptions.First(d => d.DisplayName.Contains("Уровень"));

            row.SelectedOperator = row.OperatorOptions.First(o => (CriterionOperator)o.Value == CriterionOperator.InRange);
            Check.That(row.Editor is NumberRangeVM range && range.IsRange, "диапазон дает редактор с двумя полями");

            row.SelectedOperator = row.OperatorOptions.First(o => (CriterionOperator)o.Value == CriterionOperator.Equals);
            Check.That(row.Editor is NumberRangeVM single && !single.IsRange, "равенство дает редактор с одним полем");
        }

        [PilotFact]
        public void Lookup_Category_Gets_Lookup_Editor_With_Items(ITestContext ctx)
        {
            var row = CreateRow(ctx);
            row.SelectedCategory = row.Categories.First(c => c.DisplayName == "Тип");

            Check.That(row.Editor is LookupValueVM lookup && lookup.Items.Count > 0,
                "категория Тип должна дать справочник с элементами");
        }

        [PilotFact]
        public void Empty_Row_Skipped_Invalid_Value_Reports_Error(ITestContext ctx)
        {
            var categories = new FieldCatalog(ctx.Repository).GetCategories();
            var row = new SearchCriterionVM(categories, _ => { });

            // Значение не заполнено - ошибка (строка выбрана, но пустая)
            var terms = row.TryBuildTerms(out var error);
            Check.Equal(0, terms.Count, "термов нет");
            Check.NotNull(error, "пустое значение должно давать ошибку");

            // Корректное значение - терм строится
            ((TextValueVM)row.Editor).Text = "abc";
            terms = row.TryBuildTerms(out error);
            Check.Equal(1, terms.Count, "терм построен");
            Check.That(error == null, "ошибки нет: " + error);
        }

        [PilotFact]
        public void Changed_Event_Fires_On_Editor_Input(ITestContext ctx)
        {
            var row = CreateRow(ctx);
            var fired = 0;
            row.Changed += () => fired++;

            ((TextValueVM)row.Editor).Text = "x";
            Check.That(fired > 0, "изменение значения должно поднимать Changed (живое превью)");
        }
    }
}
