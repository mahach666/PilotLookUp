using Ascon.Pilot.SDK;
using PilotLookUp.Objects;
using PilotLookUp.Objects.TypeHelpers;
using PilotTestHost.Sdk;
using System;
using System.Linq;

namespace PilotLookUp.Tests
{
    // PilotObjectMap.Wrap: каждый вид значения попадает в свою обертку
    public class TypeHelpersTests
    {
        private static PilotObjectMap Map(ITestContext ctx) => new PilotObjectMap(ctx.Repository);

        [PilotFact]
        public void Wrap_Primitives(ITestContext ctx)
        {
            var map = Map(ctx);

            Check.That(map.Wrap("строка") is StringHelper, "string → StringHelper");
            // BoolHelper внутренний - проверяем по имени типа
            Check.Equal("BoolHelper", map.Wrap(true).GetType().Name, "bool → BoolHelper");
            Check.That(map.Wrap(42L) is LongHelper, "long → LongHelper");
            Check.That(map.Wrap(DateTime.Now) is DateTimeHelper, "DateTime → DateTimeHelper");
            Check.That(map.Wrap(DataState.Loaded) is EnumHelper, "enum → EnumHelper");
            Check.That(map.Wrap(null) is NullHelper, "null → NullHelper");
        }

        [PilotFact]
        public void Wrap_Random_Guid_Is_Not_Lookable(ITestContext ctx)
        {
            var helper = Map(ctx).Wrap(Guid.NewGuid());
            Check.Equal("GuidHelper", helper.GetType().Name, "Guid → GuidHelper");
            Check.That(!helper.IsLookable, "случайный Guid некликабелен");
        }

        [PilotFact]
        public void Wrap_Real_SDK_Objects(ITestContext ctx)
        {
            var map = Map(ctx);

            var root = TestData.GetRoot(ctx);
            var rootHelper = map.Wrap(root);
            Check.That(rootHelper is DataObjectHelper, "IDataObject → DataObjectHelper");
            Check.That(rootHelper.IsLookable, "объект БД кликабелен");
            Check.Equal(root.Id.ToString(), rootHelper.StringId, "StringId объекта");

            var type = ctx.Repository.GetTypes().First();
            Check.That(map.Wrap(type) is TypeHelper, "IType → TypeHelper");

            var person = ctx.Repository.GetCurrentPerson();
            var personHelper = map.Wrap(person);
            Check.That(personHelper is PersonHelper, "IPerson → PersonHelper");
            Check.Equal(person.DisplayName, personHelper.Name, "имя человека");

            Check.Equal("ObjectsRepositoryHelper", map.Wrap(ctx.Repository).GetType().Name,
                "IObjectsRepository → ObjectsRepositoryHelper");
        }

        [PilotFact]
        public void Wrap_Unknown_Object_Is_Lookable_OtherHelper(ITestContext ctx)
        {
            // Генерический снуп: неизвестные объекты кликабельны
            var record = new MenuItemClickRecord("test");
            var helper = Map(ctx).Wrap(record);

            Check.That(helper is OtherHelper, "неизвестный тип → OtherHelper");
            Check.That(helper.IsLookable, "OtherHelper должен быть кликабелен");
        }

        [PilotFact]
        public void ObjectSet_Description_Branches(ITestContext ctx)
        {
            var empty = new ObjectSet(null);
            Check.Equal("No objects", empty.Description, "пустой набор");

            var root = TestData.GetRoot(ctx);
            var single = new ObjectSet(null) { Map(ctx).Wrap(root) };
            Check.Contains(single.Description, root.Id.ToString(), "одиночный набор показывает Id");

            var many = new ObjectSet(null) { Map(ctx).Wrap(root), Map(ctx).Wrap(root) };
            Check.Contains(many.Description, "Count = 2", "множественный набор показывает количество");

            var overridden = new ObjectSet(null) { DescriptionOverride = "своя подпись" };
            Check.Equal("своя подпись", overridden.Description, "переопределение описания");
        }

        [PilotFact]
        public void ObjectSet_Decoration_Follows_Lookability(ITestContext ctx)
        {
            var lookable = new ObjectSet(null) { Map(ctx).Wrap(TestData.GetRoot(ctx)) };
            Check.NotNull(lookable.Decoration, "кликабельный набор подчеркнут");

            var plain = new ObjectSet(null) { Map(ctx).Wrap(Guid.NewGuid()) };
            Check.That(plain.Decoration == null, "некликабельный набор без подчеркивания");
        }
    }
}
