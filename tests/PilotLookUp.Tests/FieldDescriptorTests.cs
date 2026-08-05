using Ascon.Pilot.SDK;
using PilotLookUp.Enums;
using PilotLookUp.Model.AdvancedSearch;
using PilotTestHost.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Tests
{
    // Построение поисковых термов: все фабрики дескрипторов, операторы,
    // отрицания и ошибки ввода
    public class FieldDescriptorTests
    {
        private static CriterionValue Text(string text) => new CriterionValue { Text = text };
        private static CriterionValue Range(string from, string to) => new CriterionValue { Text = from, TextTo = to };

        private static string Lucene(IReadOnlyList<ISearchTerm> terms) =>
            string.Join(" | ", terms.Select(t => t.ToLuceneString()));

        // ===== Строки =====

        [PilotFact]
        public void String_Equals_Builds_Term()
        {
            var descriptor = FieldDescriptor.ForString("Текст", ObjectFields.AllText);
            var terms = descriptor.BuildTerms(CriterionOperator.Equals, Text("тест*"));

            Check.Equal(1, terms.Count, "количество термов");
            Check.Contains(Lucene(terms), "AllText", "поле в терме");
            Check.Contains(Lucene(terms), "тест", "значение в терме");
        }

        [PilotFact]
        public void String_ContainsAll_Splits_Words()
        {
            var descriptor = FieldDescriptor.ForString("Текст", ObjectFields.AllText);
            var terms = descriptor.BuildTerms(CriterionOperator.ContainsAll, Text("один два"));

            var lucene = Lucene(terms);
            Check.Contains(lucene, "один", "первое слово");
            Check.Contains(lucene, "два", "второе слово");
        }

        [PilotFact]
        public void String_Negative_Operator_Builds_Same_Term_As_Positive()
        {
            var descriptor = FieldDescriptor.ForString("Текст", ObjectFields.AllText);

            var positive = Lucene(descriptor.BuildTerms(CriterionOperator.Equals, Text("абв")));
            var negative = Lucene(descriptor.BuildTerms(CriterionOperator.NotEquals, Text("абв")));

            Check.Equal(positive, negative, "отрицание должно строить тот же терм (минус ставит билдер)");
        }

        [PilotFact]
        public void String_Empty_Value_Throws_FormatException()
        {
            var descriptor = FieldDescriptor.ForString("Текст", ObjectFields.AllText);
            var error = Check.Throws<FormatException>(
                () => descriptor.BuildTerms(CriterionOperator.Equals, Text("  ")),
                "пустое значение");
            Check.Contains(error.Message, "не заполнено", "текст ошибки");
        }

        [PilotFact]
        public void String_Exists_Requires_No_Value()
        {
            var descriptor = FieldDescriptor.ForString("Текст", ObjectFields.AllText);
            var terms = descriptor.BuildTerms(CriterionOperator.Exists, new CriterionValue());
            Check.Equal(1, terms.Count, "Exists строится без значения");
        }

        // ===== Числа =====

        [PilotFact]
        public void Int64_Equals_And_Range()
        {
            var descriptor = FieldDescriptor.ForInt64("Размер", ObjectFields.AllFilesSizeInBytes);

            Check.Equal(1, descriptor.BuildTerms(CriterionOperator.Equals, Text("42")).Count, "Be");
            Check.Equal(1, descriptor.BuildTerms(CriterionOperator.InRange, Range("1", "100")).Count, "BeInRange");
        }

        [PilotFact]
        public void Int64_Bad_Number_Throws_With_Message()
        {
            var descriptor = FieldDescriptor.ForInt64("Размер", ObjectFields.AllFilesSizeInBytes);
            var error = Check.Throws<FormatException>(
                () => descriptor.BuildTerms(CriterionOperator.Equals, Text("abc")),
                "не-число");
            Check.Contains(error.Message, "не является целым числом", "текст ошибки");
        }

        [PilotFact]
        public void Int64_Range_Requires_Both_Bounds()
        {
            var descriptor = FieldDescriptor.ForInt64("Размер", ObjectFields.AllFilesSizeInBytes);
            Check.Throws<FormatException>(
                () => descriptor.BuildTerms(CriterionOperator.InRange, Range("1", " ")),
                "диапазон без правой границы");
        }

        [PilotFact]
        public void Double_Accepts_Comma_Separator()
        {
            var descriptor = FieldDescriptor.ForDouble("Вес", AttributeFields.Double("test_weight"));
            var terms = descriptor.BuildTerms(CriterionOperator.Equals, Text("3,14"));
            Check.Equal(1, terms.Count, "запятая как разделитель");
        }

        [PilotFact]
        public void Int32_Range_Builds()
        {
            var descriptor = FieldDescriptor.ForInt32("Уровень", ObjectFields.Level);
            var terms = descriptor.BuildTerms(CriterionOperator.InRange, Range("0", "5"));
            Check.Contains(Lucene(terms), "Level", "поле уровня");
        }

        // ===== Даты =====

        [PilotFact]
        public void Date_Range_Builds_And_Requires_Both_Dates()
        {
            var descriptor = FieldDescriptor.ForDateRange("Создан", ObjectFields.CreatedDate);

            var terms = descriptor.BuildTerms(CriterionOperator.InRange,
                new CriterionValue { DateFrom = DateTime.Today.AddDays(-7), DateTo = DateTime.Today });
            Check.Contains(Lucene(terms), "Created", "поле даты");

            Check.Throws<FormatException>(
                () => descriptor.BuildTerms(CriterionOperator.InRange, new CriterionValue { DateFrom = DateTime.Today }),
                "диапазон без второй даты");
        }

        // ===== Guid =====

        [PilotFact]
        public void Guid_Parses_And_Rejects_Garbage()
        {
            var descriptor = FieldDescriptor.ForGuid("Id", ObjectFields.Id);

            var guid = Guid.NewGuid();
            var terms = descriptor.BuildTerms(CriterionOperator.Equals, Text(guid.ToString()));
            Check.Equal(1, terms.Count, "валидный GUID");

            var error = Check.Throws<FormatException>(
                () => descriptor.BuildTerms(CriterionOperator.Equals, Text("не-гуид")),
                "мусор вместо GUID");
            Check.Contains(error.Message, "GUID", "текст ошибки");
        }

        // ===== Справочники =====

        [PilotFact]
        public void Lookup_Single_Uses_Be_Multiple_Uses_BeAnyOf()
        {
            var descriptor = FieldDescriptor.ForLookupInt32("Тип", ObjectFields.TypeId,
                () => new[] { new LookupItem(1, "один"), new LookupItem(2, "два") });

            var one = Lucene(descriptor.BuildTerms(CriterionOperator.Equals,
                new CriterionValue { SelectedLookupItems = new[] { new LookupItem(5, "пять") } }));
            var many = Lucene(descriptor.BuildTerms(CriterionOperator.Equals,
                new CriterionValue { SelectedLookupItems = new[] { new LookupItem(5, "пять"), new LookupItem(7, "семь") } }));

            Check.NotEmpty(one, "терм одиночного выбора");
            Check.That(one != many, "мультивыбор должен давать другой терм (BeAnyOf)");
        }

        [PilotFact]
        public void Lookup_Empty_Selection_Throws()
        {
            var descriptor = FieldDescriptor.ForLookupInt32("Тип", ObjectFields.TypeId, () => new LookupItem[0]);
            var error = Check.Throws<FormatException>(
                () => descriptor.BuildTerms(CriterionOperator.Equals, new CriterionValue()),
                "пустой выбор");
            Check.Contains(error.Message, "не выбрано", "текст ошибки");
        }

        [PilotFact]
        public void ObjectState_Lookup_Builds_Enum_Term()
        {
            var descriptor = FieldDescriptor.ForObjectState("Состояние", ObjectFields.ObjectState,
                () => new[] { new LookupItem(ObjectState.InRecycleBin, "В корзине") });

            var terms = descriptor.BuildTerms(CriterionOperator.Equals,
                new CriterionValue { SelectedLookupItems = new[] { new LookupItem(ObjectState.InRecycleBin, "В корзине") } });
            Check.Contains(Lucene(terms), "State", "поле состояния");
        }

        [PilotFact]
        public void Bool_Builds_Term()
        {
            var descriptor = FieldDescriptor.ForBool("Секретный", ObjectFields.IsSecret);
            var terms = descriptor.BuildTerms(CriterionOperator.Equals, new CriterionValue { BoolValue = true });
            Check.Contains(Lucene(terms), "IsSecret", "поле секретности");
        }

        // ===== «Любой атрибут ...» =====

        [PilotFact]
        public void AnyString_Builds_Term_Per_Field()
        {
            var descriptor = FieldDescriptor.ForAnyString("Любой строковый",
                new[] { AttributeFields.String("attr_a"), AttributeFields.String("attr_b"), AttributeFields.String("attr_c") });

            var terms = descriptor.BuildTerms(CriterionOperator.Equals, Text("значение"));
            Check.Equal(3, terms.Count, "терм на каждый атрибут");
        }

        [PilotFact]
        public void AnyInt64_Range_Builds_Term_Per_Field()
        {
            var descriptor = FieldDescriptor.ForAnyInt64("Любой целый",
                new[] { AttributeFields.Integer("attr_x"), AttributeFields.Integer("attr_y") });

            var terms = descriptor.BuildTerms(CriterionOperator.InRange, Range("1", "10"));
            Check.Equal(2, terms.Count, "терм на каждый атрибут");
        }

        [PilotFact]
        public void Any_With_No_Fields_Throws()
        {
            var descriptor = FieldDescriptor.ForAnyDate("Любая дата", new Ascon.Pilot.SDK.Search.Fields.DateTimeField[0]);
            var error = Check.Throws<FormatException>(
                () => descriptor.BuildTerms(CriterionOperator.InRange,
                    new CriterionValue { DateFrom = DateTime.Today, DateTo = DateTime.Today }),
                "нет атрибутов такого типа");
            Check.Contains(error.Message, "нет атрибутов", "текст ошибки");
        }

        // ===== Операторы =====

        [PilotFact]
        public void Operator_Negativity_And_Positive_Mapping()
        {
            Check.That(CriterionOperator.NotEquals.IsNegative(), "NotEquals отрицательный");
            Check.That(CriterionOperator.NotContainsAll.IsNegative(), "NotContainsAll отрицательный");
            Check.That(CriterionOperator.NotExists.IsNegative(), "NotExists отрицательный");
            Check.That(!CriterionOperator.Equals.IsNegative(), "Equals положительный");
            Check.That(!CriterionOperator.InRange.IsNegative(), "InRange положительный");

            Check.Equal(CriterionOperator.Equals, CriterionOperator.NotEquals.ToPositive(), "NotEquals→Equals");
            Check.Equal(CriterionOperator.ContainsAll, CriterionOperator.NotContainsAll.ToPositive(), "NotContainsAll→ContainsAll");
            Check.Equal(CriterionOperator.Exists, CriterionOperator.NotExists.ToPositive(), "NotExists→Exists");
            Check.Equal(CriterionOperator.InRange, CriterionOperator.InRange.ToPositive(), "InRange без пары");
        }
    }
}
