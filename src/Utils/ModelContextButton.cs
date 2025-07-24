using Ascon.Pilot.Bim.SDK.ModelTab.Menu;
using Ascon.Pilot.SDK.Menu;
using System;

namespace PilotLookUp.Utils
{
    public class ModelContextButton : IMenu<ModelContext>
    {
        private readonly Action<IMenuBuilder, ModelContext> _builder;
        private readonly Action<string> _itemClick;
        public ModelContextButton(Action<IMenuBuilder, ModelContext> builder,
            Action<string> itemClick)
        {
            _builder = builder;
            _itemClick = itemClick;
        }

        public void Build(IMenuBuilder builder, ModelContext context) =>
            _builder(builder, context);

        public void OnMenuItemClick(string name, ModelContext context) =>
            _itemClick(name);
    }
}
