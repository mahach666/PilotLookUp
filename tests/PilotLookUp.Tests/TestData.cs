using Ascon.Pilot.SDK;
using PilotLookUp.Core;
using PilotTestHost.Sdk;

namespace PilotLookUp.Tests
{
    // GetRootObject() в клиенте 26.15 бросает NotSupportedException -
    // корень грузим по известному системному id
    public static class TestData
    {
        public static IDataObject GetRoot(ITestContext ctx)
        {
            var root = TestPump.Await(
                new ObjectLoader(ctx.Repository).LoadWithTimeout(SystemObjectIds.RootObjectId, 5000));

            if (root == null)
                throw new AssertFailedException("корневой объект не загрузился по SystemObjectIds.RootObjectId");
            return root;
        }
    }
}
