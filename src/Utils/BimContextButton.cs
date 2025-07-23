using Ascon.Pilot.Bim.SDK;
using Ascon.Pilot.SDK.Menu;
using System;

namespace PilotLookUp.Utils
{
    public class BimContextButton : IMenu<IModelElementId>
    {
        private readonly Action<IMenuBuilder, IModelElementId> _builder;
        private readonly Action<string> _itemClick;
        public BimContextButton(Action<IMenuBuilder, IModelElementId> builder,
            Action<string> itemClick)
        {
            _builder = builder;
            _itemClick = itemClick;
        }

        public void Build(IMenuBuilder builder, IModelElementId context) =>
            _builder(builder, context);

        public void OnMenuItemClick(string name, IModelElementId context) =>
            _itemClick(name);
    }
}
