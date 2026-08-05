using Ascon.Pilot.SDK;
using PilotLookUp.Core;
using PilotLookUp.Extensions;
using PilotTestHost.Sdk;
using System;

namespace PilotLookUp.Tests
{
    // Загрузчики объектов: живые id и таймауты на несуществующих
    public class CoreLoadersTests
    {
        [PilotFact]
        public void ObjectLoader_Loads_Real_Object(ITestContext ctx)
        {
            var loaded = TestPump.Await(
                new ObjectLoader(ctx.Repository).LoadWithTimeout(SystemObjectIds.RootObjectId, 5000));

            Check.NotNull(loaded, "корневой объект должен загрузиться");
            Check.Equal(SystemObjectIds.RootObjectId, loaded.Id, "id загруженного объекта");
        }

        [PilotFact]
        public void ObjectLoader_Returns_Null_On_Unknown_Guid(ITestContext ctx)
        {
            var loaded = TestPump.Await(new ObjectLoader(ctx.Repository).LoadWithTimeout(Guid.NewGuid(), 500));
            Check.That(loaded == null, "несуществующий id должен дать null по таймауту");
        }

        // Проверено прогоном: GetHistoryItems(несуществующий id) блокирует UI-поток
        // внутри самого вызова SDK и не возвращается - клиентский таймаут бессилен.
        // В продукте путь безопасен: Tracer передает только id из реальных HistoryItems
        [PilotFact(Skip = "SDK: GetHistoryItems по несуществующему id намертво блокирует UI-поток клиента")]
        public void HistoryItemLoader_Returns_Null_On_Unknown_Guid(ITestContext ctx)
        {
            var loaded = TestPump.Await(new HistoryItemLoader(ctx.Repository).LoadWithTimeout(Guid.NewGuid(), 500));
            Check.That(loaded == null, "несуществующая история должна дать null по таймауту");
        }

        // У корня в этой базе есть скрытый супер-родитель, поэтому проверяем
        // не «корень возвращает сам себя», а завершаемость подъема (защита от цикла)
        [PilotFact]
        public void FindLastParent_Terminates_And_Returns_Object(ITestContext ctx)
        {
            var root = TestData.GetRoot(ctx);

            var top = TestPump.Await(root.FindLastParent(ctx.Repository));

            Check.NotNull(top, "результат подъема по родителям");
            Check.That(top.Id != System.Guid.Empty, "вершина - реальный объект");
        }
    }
}
