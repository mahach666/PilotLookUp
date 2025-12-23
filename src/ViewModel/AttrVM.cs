using Ascon.Pilot.SDK;
using PilotLookUp.Commands;
using PilotLookUp.Contracts;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.Objects.TypeHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace PilotLookUp.ViewModel
{
    public class AttrVM : INotifyPropertyChanged, IPage
    {
        private readonly PilotObjectHelper _objectHelper;
        private readonly IDataObjectService _dataObjectService;
        private readonly IRepoService _repoService;
        private readonly IViewFactory _viewFactory;
        private readonly IObjectsRepository _objectsRepository;

        public AttrVM(
            PilotObjectHelper pilotObjectHelper,
            IDataObjectService dataObjectService,
            IRepoService repoService,
            IViewFactory viewFactory,
            IObjectsRepository objectsRepository)
        {
            _objectHelper = pilotObjectHelper;
            _dataObjectService = dataObjectService;
            _repoService = repoService;
            _viewFactory = viewFactory;
            _objectsRepository = objectsRepository;
        }

        public IEnumerable<AttrDTO> Attrs
        {
            get
            {
                if (_objectHelper == null || _objectHelper.LookUpObject == null)
                    return new List<AttrDTO>();
                return _dataObjectService.GetAttrDTOs(_objectHelper as DataObjectHelper);
            }
        }

        public string IdSelectedItem => _objectHelper.StringId;

        public string NameSelectedItem => _objectHelper.Name;

        private AttrDTO _dataGridSelected;
        public AttrDTO DataGridSelected
        {
            get => _dataGridSelected;
            set
            {
                _dataGridSelected = value;
                OnPropertyChanged();
            }
        }

        private void CopyToClipboard(string sender)
        {
            var errorText = "Упс, ничего не выбрано.";
            if (_dataGridSelected == null) MessageBox.Show(errorText);

            try
            {
                if (sender == "DataGridSelectName")
                {
                    Clipboard.SetText(_dataGridSelected?.Name);
                }
                else if (sender == "DataGridSelectValue")
                {
                    Clipboard.SetText(_dataGridSelected?.Value);
                }
                else if (sender == "DataGridSelectTitle")
                {
                    Clipboard.SetText(_dataGridSelected?.Title);
                }
                else
                {
                    MessageBox.Show(errorText);
                }
            }
            catch
            {
                MessageBox.Show(errorText);
            }
        }

        public ICommand CopyCommand => new RelayCommand<string>(CopyToClipboard);

        public ICommand OpenAttrValueCommand => new RelayCommand<AttrDTO>(OpenAttrValueAsync, CanOpenAttrValue);

        private bool CanOpenAttrValue(AttrDTO attr) =>
            attr?.IsLookable == true;

        private async void OpenAttrValueAsync(AttrDTO attr)
        {
            if (attr?.IsLookable != true) return;

            try
            {
                if (attr.Type == "OrgUnit")
                {
                    var ids = ExtractOrgUnitIds(attr.RawValue, attr.Value);
                    if (ids.Count == 0) return;

                    var map = new PilotObjectMap(_objectsRepository);
                    var objectSet = new ObjectSet(null);

                    foreach (var id in ids)
                    {
                        var unit = _objectsRepository.GetOrganisationUnit(id);
                        if (unit != null)
                            objectSet.Add(map.Wrap(unit));
                    }

                    _viewFactory.LookSelection(objectSet);
                    return;
                }

                if (attr.Type == "UserState")
                {
                    var states = ExtractUserStates(attr.RawValue, attr.Value);
                    if (states.Count > 0)
                    {
                        var map = new PilotObjectMap(_objectsRepository);
                        var objectSet = new ObjectSet(null);
                        foreach (var state in states)
                            objectSet.Add(map.Wrap(state));
                        _viewFactory.LookSelection(objectSet);
                        return;
                    }
                }

                var guids = ExtractGuids(attr.RawValue, attr.Value);
                if (guids.Count == 0) return;

                var selection = await _repoService.GetWrapedObjs(guids);
                _viewFactory.LookSelection(selection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private List<Guid> ExtractGuids(object rawValue, string fallbackText)
        {
            var res = new List<Guid>();

            if (rawValue is Guid g)
                return new List<Guid> { g };

            if (rawValue is IEnumerable enumerable && rawValue is not string)
            {
                foreach (var item in enumerable.Cast<object>())
                {
                    if (item is Guid eg)
                        res.Add(eg);
                    else if (item is string es && Guid.TryParse(es, out var parsed))
                        res.Add(parsed);
                }
            }
            else if (rawValue is string s && !string.IsNullOrWhiteSpace(s))
            {
                res.AddRange(ParseGuidsFromText(s));
            }

            if (res.Count == 0 && !string.IsNullOrWhiteSpace(fallbackText))
                res.AddRange(ParseGuidsFromText(fallbackText));

            return res.Distinct().ToList();
        }

        private List<Guid> ParseGuidsFromText(string text)
        {
            var tokens = text.Split(new[] { ';', ',', '\n', '\r', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var list = new List<Guid>();
            foreach (var token in tokens)
            {
                if (Guid.TryParse(token.Trim(), out var g))
                    list.Add(g);
            }

            return list;
        }

        private List<int> ExtractOrgUnitIds(object rawValue, string fallbackText)
        {
            var res = new List<int>();

            if (rawValue is int i)
                return new List<int> { i };

            if (rawValue is IEnumerable enumerable && rawValue is not string)
            {
                foreach (var item in enumerable.Cast<object>())
                {
                    if (item is int ei)
                        res.Add(ei);
                    else if (item is string es && int.TryParse(es, out var parsed))
                        res.Add(parsed);
                }
            }
            else if (rawValue is string s && !string.IsNullOrWhiteSpace(s))
            {
                res.AddRange(ParseIntsFromText(s));
            }

            if (res.Count == 0 && !string.IsNullOrWhiteSpace(fallbackText))
                res.AddRange(ParseIntsFromText(fallbackText));

            return res.Distinct().ToList();
        }

        private List<int> ParseIntsFromText(string text)
        {
            var tokens = text.Split(new[] { ';', ',', '\n', '\r', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var list = new List<int>();
            foreach (var token in tokens)
            {
                if (int.TryParse(token.Trim(), out var i))
                    list.Add(i);
            }

            return list;
        }

        private List<IUserState> ExtractUserStates(object rawValue, string fallbackText)
        {
            var states = _objectsRepository?.GetUserStates() ?? Enumerable.Empty<IUserState>();

            if (rawValue is IUserState state)
                return new List<IUserState> { state };

            if (rawValue is Guid guid)
            {
                var byId = states.FirstOrDefault(s => s.Id == guid);
                if (byId != null) return new List<IUserState> { byId };
            }

            if (rawValue is string name && !string.IsNullOrWhiteSpace(name))
            {
                var byName = states.Where(s =>
                        string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase)
                        || string.Equals(s.Title, name, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                if (byName.Count > 0) return byName;
            }

            if (!string.IsNullOrWhiteSpace(fallbackText))
            {
                var byName = states.Where(s =>
                        string.Equals(s.Name, fallbackText, StringComparison.OrdinalIgnoreCase)
                        || string.Equals(s.Title, fallbackText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                if (byName.Count > 0) return byName;
            }

            return new List<IUserState>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        PagesName IPage.GetName() =>
            PagesName.AttrPage;
    }
}

