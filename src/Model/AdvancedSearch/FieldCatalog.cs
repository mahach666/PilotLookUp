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

        private static bool IsStringKind(IAttribute attribute) =>
            attribute.Type == AttributeType.String
            || attribute.Type == AttributeType.Numerator
            || attribute.Type == AttributeType.Array
            || attribute.Type == AttributeType.ElementBook;

        private static IEnumerable<IAttribute> AttributeFieldsOfKind(
            IReadOnlyList<IAttribute> attributes, Func<IAttribute, bool> predicate) =>
            attributes.Where(predicate);

        // Уникальные не-сервисные атрибуты по всем типам
        private IReadOnlyList<IAttribute> CollectAttributes()
        {
            var seen = new HashSet<string>();
            var result = new List<IAttribute>();

            var types = _repository.GetTypes();

            foreach (var type in types)
            {
                foreach (var attribute in type.Attributes.Where(a => !a.IsService))
                {
                    if (seen.Add(attribute.Name + "|" + attribute.Type))
                        result.Add(attribute);
                }
            }

            return result;
        }

        private FieldDescriptor BuildAttributeDescriptor(IAttribute attribute)
        {
            var title = string.IsNullOrEmpty(attribute.Title)
                ? attribute.Name
                : $"{attribute.Title} ({attribute.Name})";

            switch (attribute.Type)
            {
                case AttributeType.Integer:
                    return FieldDescriptor.ForInt64(title, AttributeFields.Integer(attribute.Name));
                case AttributeType.Double:
                case AttributeType.Decimal:
                    return FieldDescriptor.ForDouble(title, AttributeFields.Double(attribute.Name));
                case AttributeType.DateTime:
                    return FieldDescriptor.ForDateRange(title, AttributeFields.DateTime(attribute.Name));
                case AttributeType.String:
                case AttributeType.Numerator:
                case AttributeType.Array:
                case AttributeType.ElementBook:
                    return FieldDescriptor.ForString(title, AttributeFields.String(attribute.Name));
                case AttributeType.UserState:
                    return FieldDescriptor.ForLookupGuid(title, AttributeFields.State(attribute.Name), UserStateItems);
                case AttributeType.OrgUnit:
                    return FieldDescriptor.ForLookupInt32(title, AttributeFields.OrgUnit(attribute.Name), OrgUnitItems);
                case AttributeType.Boolean:
                    return FieldDescriptor.ForBool(title, AttributeFields.Bool(attribute.Name));
                default:
                    // Inherited и ByteArray не индексируются
                    return null;
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

        private IEnumerable<LookupItem> OrgUnitItems()
        {
            return _repository.GetOrganisationUnits()
                .OrderBy(u => u.Title, StringComparer.CurrentCultureIgnoreCase)
                .Select(u => new LookupItem(u.Id, u.Title));
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
