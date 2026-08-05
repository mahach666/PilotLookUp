using Ascon.Pilot.SDK;
using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Model.AdvancedSearch;
using PilotLookUp.ViewModel.Editors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace PilotLookUp.ViewModel
{
    // Строка критерия: сочетание (И/И-НЕ/ИЛИ) - категория - [детализация] - оператор - значение
    public class SearchCriterionVM : INotifyPropertyChanged
    {
        private static readonly IReadOnlyList<ChoiceItem> AllOccurOptions = new[]
        {
            new ChoiceItem(OccurKind.Must, "И"),
            new ChoiceItem(OccurKind.Should, "ИЛИ")
        };

        private static readonly Dictionary<CriterionOperator, string> OperatorTitles = new Dictionary<CriterionOperator, string>
        {
            { CriterionOperator.Equals, "Равно" },
            { CriterionOperator.NotEquals, "Не равно" },
            { CriterionOperator.ContainsAll, "Содержит слова" },
            { CriterionOperator.NotContainsAll, "Не содержит" },
            { CriterionOperator.InRange, "Диапазон" },
            { CriterionOperator.Exists, "Заполнено" },
            { CriterionOperator.NotExists, "Не заполнено" }
        };

        private readonly Action<SearchCriterionVM> _remove;

        public SearchCriterionVM(IReadOnlyList<CategoryDescriptor> categories, Action<SearchCriterionVM> remove)
        {
            Categories = categories;
            _remove = remove;
            _selectedOccur = AllOccurOptions[0];
            Editor = new NoValueVM();

            // Первая категория выбрана сразу - строка не начинается с пустых комбо
            SelectedCategory = categories?.FirstOrDefault();
        }

        // Уведомляет родителя о любом изменении строки (для живого превью)
        public event Action Changed;

        public IReadOnlyList<CategoryDescriptor> Categories { get; }
        public IReadOnlyList<ChoiceItem> OccurOptions => AllOccurOptions;

        private ChoiceItem _selectedOccur;
        public ChoiceItem SelectedOccur
        {
            get => _selectedOccur;
            set
            {
                _selectedOccur = value;
                OnPropertyChanged();
                Changed?.Invoke();
            }
        }

        public OccurKind Occur => (OccurKind)(_selectedOccur?.Value ?? OccurKind.Must);

        private CategoryDescriptor _selectedCategory;
        public CategoryDescriptor SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DetailOptions));
                OnPropertyChanged(nameof(DetailVisibility));

                // При смене категории детализация сбрасывается на первый пункт
                _selectedDetail = value?.Details?.FirstOrDefault();
                OnPropertyChanged(nameof(SelectedDetail));

                UpdateEffectiveField();
            }
        }

        public IReadOnlyList<FieldDescriptor> DetailOptions => _selectedCategory?.Details;

        public Visibility DetailVisibility =>
            _selectedCategory?.HasDetails == true ? Visibility.Visible : Visibility.Collapsed;

        private FieldDescriptor _selectedDetail;
        public FieldDescriptor SelectedDetail
        {
            get => _selectedDetail;
            set
            {
                _selectedDetail = value;
                OnPropertyChanged();
                UpdateEffectiveField();
            }
        }

        // Фактическое поле: прямое поле категории либо выбранная детализация
        private FieldDescriptor EffectiveField => _selectedCategory?.DirectField ?? _selectedDetail;

        public IReadOnlyList<ChoiceItem> OperatorOptions { get; private set; } = new List<ChoiceItem>();

        private ChoiceItem _selectedOperator;
        public ChoiceItem SelectedOperator
        {
            get => _selectedOperator;
            set
            {
                _selectedOperator = value;
                OnPropertyChanged();
                RecreateEditor();
                Changed?.Invoke();
            }
        }

        public CriterionOperator Operator => (CriterionOperator)(_selectedOperator?.Value ?? CriterionOperator.Equals);

        private object _editor;
        public object Editor
        {
            get => _editor;
            private set
            {
                if (_editor is INotifyPropertyChanged oldEditor)
                    oldEditor.PropertyChanged -= OnEditorPropertyChanged;

                _editor = value;

                if (_editor is INotifyPropertyChanged newEditor)
                    newEditor.PropertyChanged += OnEditorPropertyChanged;

                OnPropertyChanged();
            }
        }

        private ICommand _removeCommand;
        public ICommand RemoveCommand => _removeCommand ??= new RelayCommand<object>(_ => _remove(this));

        // Пустой список без ошибки - строка не заполнена (пропускается);
        // пустой список с ошибкой - некорректный ввод
        public IReadOnlyList<ISearchTerm> TryBuildTerms(out string error)
        {
            error = null;

            var field = EffectiveField;
            if (field == null)
                return Array.Empty<ISearchTerm>();

            try
            {
                return field.BuildTerms(Operator, CollectValue());
            }
            catch (Exception e)
            {
                error = $"{field.DisplayName}: {e.Message}";
                return Array.Empty<ISearchTerm>();
            }
        }

        private CriterionValue CollectValue()
        {
            return Editor switch
            {
                TextValueVM text => new CriterionValue { Text = text.Text },
                NumberRangeVM number => new CriterionValue { Text = number.From, TextTo = number.To },
                DateRangeVM date => new CriterionValue { DateFrom = date.From, DateTo = date.To },
                BoolValueVM boolean => new CriterionValue { BoolValue = boolean.Value },
                LookupValueVM lookup => new CriterionValue { SelectedLookupItems = lookup.GetSelectedLookupItems() },
                _ => new CriterionValue()
            };
        }

        private void UpdateEffectiveField()
        {
            var field = EffectiveField;

            OperatorOptions = field?.Operators
                .Select(op => new ChoiceItem(op, OperatorTitles[op]))
                .ToList() ?? new List<ChoiceItem>();
            OnPropertyChanged(nameof(OperatorOptions));

            SelectedOperator = OperatorOptions.FirstOrDefault();
        }

        private void RecreateEditor()
        {
            var field = EffectiveField;
            var positiveOperator = Operator.ToPositive();

            if (field == null || positiveOperator == CriterionOperator.Exists)
            {
                Editor = new NoValueVM();
                return;
            }

            Editor = field.EditorKind switch
            {
                ValueEditorKind.Text => new TextValueVM(BuildHint(positiveOperator)),
                ValueEditorKind.Number => new NumberRangeVM(positiveOperator == CriterionOperator.InRange),
                ValueEditorKind.DateRange => new DateRangeVM(),
                ValueEditorKind.Bool => new BoolValueVM(),
                ValueEditorKind.Lookup => new LookupValueVM(field.LookupItemsProvider?.Invoke()),
                _ => new NoValueVM()
            };
        }

        private static string BuildHint(CriterionOperator positiveOperator)
        {
            // Строковые поля поддерживают wildcard-символы Lucene
            return positiveOperator == CriterionOperator.Equals || positiveOperator == CriterionOperator.ContainsAll
                ? "поддерживаются * и ?"
                : null;
        }

        private void OnEditorPropertyChanged(object sender, PropertyChangedEventArgs e) => Changed?.Invoke();

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
