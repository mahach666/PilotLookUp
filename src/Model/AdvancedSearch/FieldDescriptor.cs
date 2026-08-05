using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Search.Fields;
using PilotLookUp.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PilotLookUp.Model.AdvancedSearch
{
    // Поле поиска: редактор значения, допустимые операторы и построение термов.
    // Обычное поле дает один терм; поля «любой атрибут ...» - по терму на каждый
    // атрибут (собираются в MustAnyOf). Ошибки ввода - FormatException с текстом
    public sealed class FieldDescriptor : ISearchableText
    {
        private FieldDescriptor(
            string displayName,
            ValueEditorKind editorKind,
            IReadOnlyList<CriterionOperator> operators,
            Func<CriterionOperator, CriterionValue, IReadOnlyList<ISearchTerm>> buildTerms,
            Func<IEnumerable<LookupItem>> lookupItemsProvider = null,
            string searchText = null)
        {
            DisplayName = displayName;
            EditorKind = editorKind;
            Operators = operators;
            _buildTerms = buildTerms;
            LookupItemsProvider = lookupItemsProvider;
            SearchText = searchText ?? displayName;
        }

        private readonly Func<CriterionOperator, CriterionValue, IReadOnlyList<ISearchTerm>> _buildTerms;

        public string DisplayName { get; }
        public ValueEditorKind EditorKind { get; }
        public IReadOnlyList<CriterionOperator> Operators { get; }
        public Func<IEnumerable<LookupItem>> LookupItemsProvider { get; }

        // Текст для фильтра выпадашки: помимо подписи включает прочие заголовки
        // атрибута, чтобы он находился и по названию из другого типа
        public string SearchText { get; }

        public IReadOnlyList<ISearchTerm> BuildTerms(CriterionOperator op, CriterionValue value) =>
            _buildTerms(op.ToPositive(), value);

        public override string ToString() => DisplayName;

        private static IReadOnlyList<ISearchTerm> One(ISearchTerm term) => new[] { term };

        // ===== Обычные поля (один терм) =====

        public static FieldDescriptor ForString(string title, StringField field, string searchText = null)
        {
            return new FieldDescriptor(title, ValueEditorKind.Text,
                new[]
                {
                    CriterionOperator.Equals, CriterionOperator.NotEquals,
                    CriterionOperator.ContainsAll, CriterionOperator.NotContainsAll,
                    CriterionOperator.Exists, CriterionOperator.NotExists
                },
                (op, v) => One(op switch
                {
                    CriterionOperator.Equals => field.Be(RequireText(v)),
                    CriterionOperator.ContainsAll => field.ContainsAll(SplitWords(RequireText(v))),
                    _ => field.Exists()
                }), null, searchText);
        }

        public static FieldDescriptor ForDateRange(string title, DateTimeField field, string searchText = null)
        {
            return new FieldDescriptor(title, ValueEditorKind.DateRange,
                new[] { CriterionOperator.InRange, CriterionOperator.Exists, CriterionOperator.NotExists },
                (op, v) => One(op == CriterionOperator.Exists
                    ? field.Exists()
                    : BuildDateRangeTerm(field, v)), null, searchText);
        }

        public static FieldDescriptor ForInt32(string title, Int32Field field, string searchText = null)
        {
            return new FieldDescriptor(title, ValueEditorKind.Number,
                new[]
                {
                    CriterionOperator.Equals, CriterionOperator.NotEquals,
                    CriterionOperator.InRange,
                    CriterionOperator.Exists, CriterionOperator.NotExists
                },
                (op, v) => One(op switch
                {
                    CriterionOperator.Equals => field.Be(ParseInt32(RequireText(v))),
                    CriterionOperator.InRange => field.BeInRange(ParseInt32(RequireText(v)), ParseInt32(RequireTextTo(v))),
                    _ => field.Exists()
                }), null, searchText);
        }

        public static FieldDescriptor ForInt64(string title, Int64Field field, string searchText = null)
        {
            return new FieldDescriptor(title, ValueEditorKind.Number,
                new[]
                {
                    CriterionOperator.Equals, CriterionOperator.NotEquals,
                    CriterionOperator.InRange,
                    CriterionOperator.Exists, CriterionOperator.NotExists
                },
                (op, v) => One(op switch
                {
                    CriterionOperator.Equals => field.Be(ParseInt64(RequireText(v))),
                    CriterionOperator.InRange => field.BeInRange(ParseInt64(RequireText(v)), ParseInt64(RequireTextTo(v))),
                    _ => field.Exists()
                }), null, searchText);
        }

        public static FieldDescriptor ForDouble(string title, DoubleField field, string searchText = null)
        {
            return new FieldDescriptor(title, ValueEditorKind.Number,
                new[]
                {
                    CriterionOperator.Equals, CriterionOperator.NotEquals,
                    CriterionOperator.InRange,
                    CriterionOperator.Exists, CriterionOperator.NotExists
                },
                (op, v) => One(op switch
                {
                    CriterionOperator.Equals => field.Be(ParseDouble(RequireText(v))),
                    CriterionOperator.InRange => field.BeInRange(ParseDouble(RequireText(v)), ParseDouble(RequireTextTo(v))),
                    _ => field.Exists()
                }), null, searchText);
        }

        public static FieldDescriptor ForBool(string title, BoolField field, string searchText = null)
        {
            return new FieldDescriptor(title, ValueEditorKind.Bool,
                new[] { CriterionOperator.Equals, CriterionOperator.Exists, CriterionOperator.NotExists },
                (op, v) => One(op == CriterionOperator.Equals ? field.Be(v.BoolValue) : field.Exists()), null, searchText);
        }

        public static FieldDescriptor ForGuid(string title, GuidField field, string searchText = null)
        {
            return new FieldDescriptor(title, ValueEditorKind.Text,
                new[]
                {
                    CriterionOperator.Equals, CriterionOperator.NotEquals,
                    CriterionOperator.Exists, CriterionOperator.NotExists
                },
                (op, v) => One(op == CriterionOperator.Exists
                    ? field.Exists()
                    : field.Be(ParseGuid(RequireText(v)))), null, searchText);
        }

        public static FieldDescriptor ForLookupInt32(string title, Int32Field field,
            Func<IEnumerable<LookupItem>> itemsProvider, string searchText = null)
        {
            return new FieldDescriptor(title, ValueEditorKind.Lookup,
                new[]
                {
                    CriterionOperator.Equals, CriterionOperator.NotEquals,
                    CriterionOperator.Exists, CriterionOperator.NotExists
                },
                (op, v) =>
                {
                    if (op == CriterionOperator.Exists)
                        return One(field.Exists());

                    var keys = RequireLookup(v).Select(i => (int)i.Key).ToArray();
                    return One(keys.Length == 1 ? field.Be(keys[0]) : field.BeAnyOf(keys));
                },
                itemsProvider, searchText);
        }

        public static FieldDescriptor ForLookupGuid(string title, GuidField field,
            Func<IEnumerable<LookupItem>> itemsProvider, string searchText = null)
        {
            return new FieldDescriptor(title, ValueEditorKind.Lookup,
                new[]
                {
                    CriterionOperator.Equals, CriterionOperator.NotEquals,
                    CriterionOperator.Exists, CriterionOperator.NotExists
                },
                (op, v) =>
                {
                    if (op == CriterionOperator.Exists)
                        return One(field.Exists());

                    var keys = RequireLookup(v).Select(i => (Guid)i.Key).ToArray();
                    return One(keys.Length == 1 ? field.Be(keys[0]) : field.BeAnyOf(keys));
                },
                itemsProvider, searchText);
        }

        public static FieldDescriptor ForObjectState(string title, EnumField<ObjectState> field,
            Func<IEnumerable<LookupItem>> itemsProvider)
        {
            return new FieldDescriptor(title, ValueEditorKind.Lookup,
                new[]
                {
                    CriterionOperator.Equals, CriterionOperator.NotEquals,
                    CriterionOperator.Exists, CriterionOperator.NotExists
                },
                (op, v) =>
                {
                    if (op == CriterionOperator.Exists)
                        return One(field.Exists());

                    var keys = RequireLookup(v).Select(i => (ObjectState)i.Key).ToArray();
                    return One(keys.Length == 1 ? field.Be(keys[0]) : field.BeAnyOf(keys));
                },
                itemsProvider);
        }

        // ===== «Любой атрибут ...»: терм на каждый атрибут, объединяются как ИЛИ =====

        public static FieldDescriptor ForAnyString(string title, IReadOnlyList<StringField> fields)
        {
            return new FieldDescriptor(title, ValueEditorKind.Text,
                new[]
                {
                    CriterionOperator.Equals, CriterionOperator.NotEquals,
                    CriterionOperator.ContainsAll, CriterionOperator.NotContainsAll
                },
                (op, v) =>
                {
                    RequireFields(fields);
                    var text = RequireText(v);
                    return op == CriterionOperator.Equals
                        ? fields.Select(f => f.Be(text)).ToList()
                        : fields.Select(f => (ISearchTerm)f.ContainsAll(SplitWords(text))).ToList();
                });
        }

        public static FieldDescriptor ForAnyInt64(string title, IReadOnlyList<Int64Field> fields)
        {
            return new FieldDescriptor(title, ValueEditorKind.Number,
                new[] { CriterionOperator.Equals, CriterionOperator.NotEquals, CriterionOperator.InRange },
                (op, v) =>
                {
                    RequireFields(fields);
                    if (op == CriterionOperator.Equals)
                    {
                        var value = ParseInt64(RequireText(v));
                        return fields.Select(f => f.Be(value)).ToList();
                    }

                    var from = ParseInt64(RequireText(v));
                    var to = ParseInt64(RequireTextTo(v));
                    return fields.Select(f => f.BeInRange(from, to)).ToList();
                });
        }

        public static FieldDescriptor ForAnyDouble(string title, IReadOnlyList<DoubleField> fields)
        {
            return new FieldDescriptor(title, ValueEditorKind.Number,
                new[] { CriterionOperator.Equals, CriterionOperator.NotEquals, CriterionOperator.InRange },
                (op, v) =>
                {
                    RequireFields(fields);
                    if (op == CriterionOperator.Equals)
                    {
                        var value = ParseDouble(RequireText(v));
                        return fields.Select(f => f.Be(value)).ToList();
                    }

                    var from = ParseDouble(RequireText(v));
                    var to = ParseDouble(RequireTextTo(v));
                    return fields.Select(f => f.BeInRange(from, to)).ToList();
                });
        }

        public static FieldDescriptor ForAnyDate(string title, IReadOnlyList<DateTimeField> fields)
        {
            return new FieldDescriptor(title, ValueEditorKind.DateRange,
                new[] { CriterionOperator.InRange },
                (op, v) =>
                {
                    RequireFields(fields);
                    return fields.Select(f => BuildDateRangeTerm(f, v)).ToList();
                });
        }

        // ===== Разбор значений =====

        private static ISearchTerm BuildDateRangeTerm(DateTimeField field, CriterionValue v)
        {
            if (v.DateFrom == null || v.DateTo == null)
                throw new FormatException("укажите обе даты диапазона");

            // SDK требует UTC; диапазон - полные локальные сутки включительно
            var fromUtc = v.DateFrom.Value.Date.ToUniversalTime();
            var toUtc = v.DateTo.Value.Date.AddDays(1).AddSeconds(-1).ToUniversalTime();
            return field.BeInRange(fromUtc, toUtc);
        }

        private static void RequireFields(IReadOnlyList<object> fields)
        {
            if (fields == null || fields.Count == 0)
                throw new FormatException("в базе нет атрибутов такого типа");
        }

        private static string RequireText(CriterionValue v)
        {
            if (string.IsNullOrWhiteSpace(v.Text))
                throw new FormatException("значение не заполнено");
            return v.Text.Trim();
        }

        private static string RequireTextTo(CriterionValue v)
        {
            if (string.IsNullOrWhiteSpace(v.TextTo))
                throw new FormatException("не заполнена граница диапазона");
            return v.TextTo.Trim();
        }

        private static IReadOnlyList<LookupItem> RequireLookup(CriterionValue v)
        {
            if (v.SelectedLookupItems == null || v.SelectedLookupItems.Count == 0)
                throw new FormatException("не выбрано значение из списка");
            return v.SelectedLookupItems;
        }

        private static string[] SplitWords(string text) =>
            text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        private static Guid ParseGuid(string text) =>
            Guid.TryParse(text, out var value) ? value : throw new FormatException("значение не является GUID");

        private static int ParseInt32(string text) =>
            int.TryParse(text, out var value) ? value : throw new FormatException($"'{text}' не является целым числом");

        private static long ParseInt64(string text) =>
            long.TryParse(text, out var value) ? value : throw new FormatException($"'{text}' не является целым числом");

        private static double ParseDouble(string text) =>
            double.TryParse(text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
                ? value
                : throw new FormatException($"'{text}' не является числом");
    }
}
