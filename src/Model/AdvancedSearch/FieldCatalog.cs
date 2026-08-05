using Ascon.Pilot.SDK;
using Ascon.Pilot.SDK.Search.Fields;
using PilotLookUp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Model.AdvancedSearch
{
    // Каталог категорий расширенного поиска: короткий список основных категорий,
    // детализация (конкретный атрибут / системное поле) - во втором комбо
    public class FieldCatalog
    {
        private readonly IObjectsRepository _repository;

        public FieldCatalog(IObjectsRepository repository)
        {
            _repository = repository;
        }

        // Область поиска не выбирается: ищем по всей базе (пустой билдер),
        // сужение - критериями «Тип» и «Состояние объекта»
        public List<CategoryDescriptor> GetCategories()
        {
            return new List<CategoryDescriptor>
            {
                CategoryDescriptor.Direct("Текст", FieldDescriptor.ForString("Весь текст", ObjectFields.AllText)),
                CategoryDescriptor.Direct("Тип", FieldDescriptor.ForLookupInt32("Тип", ObjectFields.TypeId, TypeItems)),
                CategoryDescriptor.WithDetails("Атрибут", BuildAttributeDetails()),
                CategoryDescriptor.Direct("Автор", FieldDescriptor.ForLookupInt32("Автор", ObjectFields.CreatorId, PeopleItems)),
                CategoryDescriptor.Direct("Дата создания", FieldDescriptor.ForDateRange("Дата создания", ObjectFields.CreatedDate)),
                CategoryDescriptor.Direct("Дата изменения", FieldDescriptor.ForDateRange("Дата изменения", ObjectFields.ModifiedDate)),
                CategoryDescriptor.Direct("Состояние объекта", FieldDescriptor.ForObjectState("Состояние объекта", ObjectFields.ObjectState, ObjectStateItems)),
                CategoryDescriptor.Direct("Секретность", FieldDescriptor.ForBool("Секретный", ObjectFields.IsSecret)),
                CategoryDescriptor.Direct("Id объекта", FieldDescriptor.ForGuid("Id объекта", ObjectFields.Id)),
                CategoryDescriptor.WithDetails("Системное поле", BuildSystemFieldDetails())
            };
        }

        // ===== Детализация «Атрибут»: спец-пункты «Любой ...» + конкретные атрибуты =====

        private IReadOnlyList<FieldDescriptor> BuildAttributeDetails()
        {
            var attributes = CollectAttributes();

            var details = new List<FieldDescriptor>
            {
                FieldDescriptor.ForAnyString("Любой строковый атрибут",
                    AttributeFieldsOfKind(attributes, IsStringKind).Select(a => AttributeFields.String(a.Name)).ToList()),
                FieldDescriptor.ForAnyInt64("Любой целочисленный атрибут",
                    AttributeFieldsOfKind(attributes, a => a.Type == AttributeType.Integer).Select(a => AttributeFields.Integer(a.Name)).ToList()),
                FieldDescriptor.ForAnyDouble("Любой дробный атрибут",
                    AttributeFieldsOfKind(attributes, a => a.Type == AttributeType.Double || a.Type == AttributeType.Decimal).Select(a => AttributeFields.Double(a.Name)).ToList()),
                FieldDescriptor.ForAnyDate("Любой атрибут-дата",
                    AttributeFieldsOfKind(attributes, a => a.Type == AttributeType.DateTime).Select(a => AttributeFields.DateTime(a.Name)).ToList())
            };

            details.AddRange(attributes
                .Select(BuildAttributeDescriptor)
                .Where(d => d != null)
                .OrderBy(d => d.DisplayName, StringComparer.CurrentCultureIgnoreCase));

            return details;
        }

        private static bool IsStringKind(AttributeGroup attribute) =>
            attribute.Type == AttributeType.String
            || attribute.Type == AttributeType.Numerator
            || attribute.Type == AttributeType.Array
            || attribute.Type == AttributeType.ElementBook;

        private static IEnumerable<AttributeGroup> AttributeFieldsOfKind(
            IReadOnlyList<AttributeGroup> attributes, Func<AttributeGroup, bool> predicate) =>
            attributes.Where(predicate);

        // Не-сервисные атрибуты по всем типам, схлопнутые по (имя, тип).
        // Один и тот же атрибут в разных типах может иметь разные заголовки
        // и разные машины состояний - сохраняем все варианты
        private IReadOnlyList<AttributeGroup> CollectAttributes()
        {
            var groups = new Dictionary<string, AttributeGroup>();

            foreach (var type in _repository.GetTypes())
            {
                foreach (var attribute in type.Attributes.Where(a => !a.IsService))
                {
                    var key = attribute.Name + "|" + attribute.Type;

                    if (!groups.TryGetValue(key, out var group))
                        groups[key] = group = new AttributeGroup(attribute.Name, attribute.Type);

                    group.Add(attribute);
                }
            }

            return groups.Values.ToList();
        }

        private FieldDescriptor BuildAttributeDescriptor(AttributeGroup attribute)
        {
            // Заголовок берем самый распространенный: у grif это «Гриф» (783 типа),
            // а не «Гриф конфиденциальности» (24 типа). Тип в подписи нужен, потому что
            // в базе есть одноименные атрибуты разных типов (grif - и строка, и состояние)
            var name = string.IsNullOrEmpty(attribute.MainTitle)
                ? attribute.Name
                : $"{attribute.MainTitle} ({attribute.Name})";
            var title = $"{name} · {TypeTitle(attribute.Type)}";

            // Прочие заголовки этого атрибута ищутся, но список не засоряют
            var searchText = attribute.Titles.Count > 1
                ? title + " " + string.Join(" ", attribute.Titles)
                : null;

            switch (attribute.Type)
            {
                case AttributeType.Integer:
                    return FieldDescriptor.ForInt64(title, AttributeFields.Integer(attribute.Name), searchText);
                case AttributeType.Double:
                case AttributeType.Decimal:
                    return FieldDescriptor.ForDouble(title, AttributeFields.Double(attribute.Name), searchText);
                case AttributeType.DateTime:
                    return FieldDescriptor.ForDateRange(title, AttributeFields.DateTime(attribute.Name), searchText);
                case AttributeType.String:
                case AttributeType.Numerator:
                case AttributeType.Array:
                case AttributeType.ElementBook:
                    return FieldDescriptor.ForString(title, AttributeFields.String(attribute.Name), searchText);
                case AttributeType.UserState:
                    return FieldDescriptor.ForLookupGuid(title, AttributeFields.State(attribute.Name),
                        () => UserStatesOfAttribute(attribute), searchText);
                case AttributeType.OrgUnit:
                    return FieldDescriptor.ForLookupInt32(title, AttributeFields.OrgUnit(attribute.Name),
                        OrgUnitItems, searchText);
                case AttributeType.Boolean:
                    return FieldDescriptor.ForBool(title, AttributeFields.Bool(attribute.Name), searchText);
                default:
                    // Inherited и ByteArray не индексируются
                    return null;
            }
        }

        private static string TypeTitle(AttributeType type)
        {
            switch (type)
            {
                case AttributeType.Integer: return "целое";
                case AttributeType.Double:
                case AttributeType.Decimal: return "дробное";
                case AttributeType.DateTime: return "дата";
                case AttributeType.String: return "строка";
                case AttributeType.Numerator: return "нумератор";
                case AttributeType.Array: return "массив";
                case AttributeType.ElementBook: return "справочник";
                case AttributeType.UserState: return "состояние";
                case AttributeType.OrgUnit: return "оргединица";
                case AttributeType.Boolean: return "да/нет";
                default: return type.ToString();
            }
        }

        // ===== Детализация «Системное поле»: остальные поля объекта =====

        private static IReadOnlyList<FieldDescriptor> BuildSystemFieldDetails()
        {
            return new List<FieldDescriptor>
            {
                FieldDescriptor.ForGuid("Id родителя", ObjectFields.ParentId),
                FieldDescriptor.ForString("Тег задачи", TaskFields.Tag),
                FieldDescriptor.ForDateRange("Дата смены состояния", ObjectFields.StateDate),
                FieldDescriptor.ForGuid("Id состояния (StateId)", ObjectFields.StateId),
                FieldDescriptor.ForInt64("Размер всех файлов, байт", ObjectFields.AllFilesSizeInBytes),
                FieldDescriptor.ForInt64("Размер файлов актуального снапшота, байт", ObjectFields.AllFilesInActualSnapshotSizeInBytes),
                FieldDescriptor.ForInt64("Наибольший файл снапшота, байт", ObjectFields.LargestFileInActualSnapshotSizeInBytes),
                FieldDescriptor.ForInt32("Уровень в иерархии", ObjectFields.Level),
                FieldDescriptor.ForString("Причина снапшота", ObjectFields.AllSnapshotsReason),
                FieldDescriptor.ForDateRange("Дата снапшота", ObjectFields.SnapshotsCreated),
                FieldDescriptor.ForGuid("Контекст", ObjectFields.Context)
            };
        }

        // ===== Справочники значений =====

        private IEnumerable<LookupItem> TypeItems()
        {
            return _repository.GetTypes()
                .OrderBy(t => t.Title, StringComparer.CurrentCultureIgnoreCase)
                .Select(t => new LookupItem(t.Id, t.IsService ? $"{t.Title} (сервисный)" : t.Title));
        }

        private IEnumerable<LookupItem> PeopleItems()
        {
            return _repository.GetPeople()
                .OrderBy(p => p.DisplayName, StringComparer.CurrentCultureIgnoreCase)
                .Select(p => new LookupItem(p.Id, p.DisplayName));
        }

        // Должность показывается вместе с ФИО занимающего ее человека:
        // по одним названиям должностей («Новая должность», «bot») найти человека нельзя
        private IEnumerable<LookupItem> OrgUnitItems()
        {
            var peopleByPosition = new Dictionary<int, List<string>>();

            foreach (var person in _repository.GetPeople())
            {
                foreach (var position in person.Positions)
                {
                    if (!peopleByPosition.TryGetValue(position.Position, out var names))
                        peopleByPosition[position.Position] = names = new List<string>();
                    names.Add(person.DisplayName);
                }
            }

            return _repository.GetOrganisationUnits()
                .Select(u => new LookupItem(u.Id, BuildOrgUnitTitle(u, peopleByPosition)))
                .OrderBy(i => i.Title, StringComparer.CurrentCultureIgnoreCase);
        }

        private static string BuildOrgUnitTitle(IOrganisationUnit unit, IDictionary<int, List<string>> peopleByPosition)
        {
            var title = unit.Title;

            if (peopleByPosition.TryGetValue(unit.Id, out var names) && names.Count > 0)
                title = string.Join(", ", names) + " - " + title;

            return unit.IsDeleted ? title + " (удалена)" : title;
        }

        // Состояния конкретного атрибута: Configuration хранит id машины состояний.
        // У одного атрибута в разных типах машины могут быть разными (status_doc - три),
        // поэтому берем объединение: искать можно по любому допустимому состоянию
        private IEnumerable<LookupItem> UserStatesOfAttribute(AttributeGroup attribute)
        {
            var machines = _repository.GetUserStateMachines().ToDictionary(m => m.Id);
            var states = _repository.GetUserStates().ToDictionary(s => s.Id);
            var items = new Dictionary<Guid, LookupItem>();

            foreach (var configuration in attribute.Configurations)
            {
                if (!Guid.TryParse(configuration, out var machineId))
                    continue;
                if (!machines.TryGetValue(machineId, out var machine))
                    continue;

                foreach (var stateId in machine.StateTransitions.Keys)
                {
                    if (states.TryGetValue(stateId, out var state) && !items.ContainsKey(stateId))
                        items[stateId] = new LookupItem(stateId, state.Title);
                }
            }

            if (items.Count == 0)
                return UserStateItems(); // машины состояний не найдены - все состояния базы

            return items.Values.OrderBy(i => i.Title, StringComparer.CurrentCultureIgnoreCase).ToList();
        }

        private IEnumerable<LookupItem> UserStateItems()
        {
            return _repository.GetUserStates()
                .OrderBy(s => s.Title, StringComparer.CurrentCultureIgnoreCase)
                .Select(s => new LookupItem(s.Id, s.Title));
        }

        private static IEnumerable<LookupItem> ObjectStateItems()
        {
            yield return new LookupItem(ObjectState.Alive, "Действует");
            yield return new LookupItem(ObjectState.InRecycleBin, "В корзине");
            yield return new LookupItem(ObjectState.DeletedPermanently, "Удален безвозвратно");
            yield return new LookupItem(ObjectState.Frozen, "Заморожен");
            yield return new LookupItem(ObjectState.LockRequested, "Запрошена блокировка");
            yield return new LookupItem(ObjectState.LockAccepted, "Заблокирован");
        }
    }
}
