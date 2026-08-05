using System.Collections.Generic;

namespace PilotLookUp.Model.AdvancedSearch
{
    // Категория поиска в первой выпадашке. Либо прямое поле (Тип, Автор, Дата...),
    // либо категория с детализацией во втором комбо (Атрибут, Системное поле)
    public sealed class CategoryDescriptor
    {
        private CategoryDescriptor(string displayName, FieldDescriptor directField, IReadOnlyList<FieldDescriptor> details)
        {
            DisplayName = displayName;
            DirectField = directField;
            Details = details;
        }

        public string DisplayName { get; }
        public FieldDescriptor DirectField { get; }
        public IReadOnlyList<FieldDescriptor> Details { get; }
        public bool HasDetails => Details != null;

        public override string ToString() => DisplayName;

        public static CategoryDescriptor Direct(string displayName, FieldDescriptor field) =>
            new CategoryDescriptor(displayName, field, null);

        public static CategoryDescriptor WithDetails(string displayName, IReadOnlyList<FieldDescriptor> details) =>
            new CategoryDescriptor(displayName, null, details);
    }
}
