using Ascon.Pilot.SDK;
using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Model.AdvancedSearch;
using PilotLookUp.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace PilotLookUp.ViewModel
{
    // Расширенный поиск: критерии по полям/атрибутам + сырой Lucene,
    // область Объекты/Задачи/Всё, живое превью запроса
    public class AdvancedSearchVM : INotifyPropertyChanged
    {
        private const int DefaultMaxResults = 100;
        private const int MaxResultsLimit = 1000;

        private readonly IAdvancedSearchService _advancedSearchService;
        private readonly IRepoService _repoService;
        private readonly Action<ObjectSet> _publishResults;

        public AdvancedSearchVM(
            IAdvancedSearchService advancedSearchService,
            IRepoService repoService,
            Action<ObjectSet> publishResults)
        {
            _advancedSearchService = advancedSearchService;
            _repoService = repoService;
            _publishResults = publishResults;

            Criteria = new ObservableCollection<SearchCriterionVM>();

            if (IsAvailable)
                AddCriterion();
        }

        public bool IsAvailable => _advancedSearchService.IsAvailable;

        public ObservableCollection<SearchCriterionVM> Criteria { get; }

        private string _rawLuceneQuery;
        public string RawLuceneQuery
        {
            get => _rawLuceneQuery;
            set
            {
                _rawLuceneQuery = value;
                OnPropertyChanged();
                RefreshPreview();
            }
        }

        private string _maxResultsText = DefaultMaxResults.ToString();
        public string MaxResultsText
        {
            get => _maxResultsText;
            set
            {
                _maxResultsText = value;
                OnPropertyChanged();
                RefreshPreview();
            }
        }

        private string _preview;
        public string Preview
        {
            get => _preview;
            private set { _preview = value; OnPropertyChanged(); }
        }

        private string _resultsSummary;
        public string ResultsSummary
        {
            get => _resultsSummary;
            private set { _resultsSummary = value; OnPropertyChanged(); }
        }

        private bool _isSearching;
        public bool IsSearching
        {
            get => _isSearching;
            private set { _isSearching = value; OnPropertyChanged(); }
        }

        private ICommand _addCriterionCommand;
        public ICommand AddCriterionCommand => _addCriterionCommand ??= new RelayCommand<object>(_ => AddCriterion());

        private ICommand _runSearchCommand;
        public ICommand RunSearchCommand =>
            _runSearchCommand ??= new RelayCommand<object>(_ => RunSearch(), _ => !IsSearching);

        private ICommand _clearCriteriaCommand;
        public ICommand ClearCriteriaCommand =>
            _clearCriteriaCommand ??= new RelayCommand<object>(_ => ClearCriteria(), _ => Criteria.Count > 0);

        // Сырой Lucene-запрос не трогаем: он в свернутой панели, и его молчаливая
        // очистка была бы неожиданной
        private void ClearCriteria()
        {
            foreach (var criterion in Criteria)
                criterion.Changed -= RefreshPreview;

            Criteria.Clear();
            ResultsSummary = null;
            AddCriterion();
        }

        private void AddCriterion()
        {
            var criterion = new SearchCriterionVM(_advancedSearchService.GetCategories(), RemoveCriterion);
            criterion.Changed += RefreshPreview;
            Criteria.Add(criterion);
            RefreshPreview();
        }

        private void RemoveCriterion(SearchCriterionVM criterion)
        {
            criterion.Changed -= RefreshPreview;
            Criteria.Remove(criterion);
            RefreshPreview();
        }

        private void RefreshPreview()
        {
            try
            {
                var (lucene, typeFilter) = BuildQuery(ParseMaxResults()).Build();
                Preview = string.IsNullOrEmpty(typeFilter) ? lucene : $"{lucene}   [фильтр: {typeFilter}]";
            }
            catch (Exception e)
            {
                Preview = "Запрос не собран: " + e.Message;
            }
        }

        private async void RunSearch()
        {
            // Строки с некорректным вводом блокируют запуск (ошибка должна быть видна,
            // а не молча выброшена из запроса)
            var errors = new List<string>();
            foreach (var criterion in Criteria)
            {
                criterion.TryBuildTerms(out var error);
                if (error != null)
                    errors.Add(error);
            }

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join(Environment.NewLine, errors), "Проверьте условия поиска");
                return;
            }

            IsSearching = true;
            try
            {
                var maxResults = ParseMaxResults();
                var result = await _advancedSearchService.RunAsync(BuildQuery(maxResults), maxResults);
                var objectSet = await _repoService.GetWrapedObjs(result.Ids);

                _publishResults(objectSet);

                ResultsSummary = $"Показано {result.Ids.Count} из {result.Total}"
                    + (result.TimedOut ? " (таймаут, результат неполный)" : "");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка поиска");
            }
            finally
            {
                IsSearching = false;
            }
        }

        // Билдер одноразовый: собирается заново и для превью, и для запуска
        private IQueryBuilder BuildQuery(int maxResults)
        {
            var builder = _advancedSearchService.CreateBuilder();
            var shouldTerms = new List<ISearchTerm>();

            foreach (var criterion in Criteria)
            {
                var terms = criterion.TryBuildTerms(out _);
                if (terms.Count == 0)
                    continue;

                // Отрицательный оператор («Не равно»...) сильнее сочетания:
                // такая строка всегда исключающая, ИЛИ к ней неприменимо
                if (criterion.Operator.IsNegative())
                {
                    if (terms.Count == 1)
                        builder.MustNot(terms[0]);
                    else
                        builder.MustNotAnyOf(terms.ToArray());
                    continue;
                }

                // Несколько термов у одной строки («любой атрибут ...») - это ИЛИ внутри строки
                if (criterion.Occur == OccurKind.Should)
                {
                    shouldTerms.AddRange(terms);
                }
                else if (terms.Count == 1)
                {
                    builder.Must(terms[0]);
                }
                else
                {
                    builder.MustAnyOf(terms.ToArray());
                }
            }

            if (shouldTerms.Count == 1)
                builder.Must(shouldTerms[0]);
            else if (shouldTerms.Count > 1)
                builder.MustAnyOf(shouldTerms.ToArray());

            if (!string.IsNullOrWhiteSpace(RawLuceneQuery))
                builder.AppendPlainQuery(RawLuceneQuery.Trim());

            builder.MaxResults(maxResults);
            builder.SortBy(ObjectFields.ModifiedDate, ListSortDirection.Descending);

            return builder;
        }

        private int ParseMaxResults()
        {
            if (!int.TryParse(_maxResultsText, out var value))
                return DefaultMaxResults;
            return Math.Max(1, Math.Min(MaxResultsLimit, value));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
