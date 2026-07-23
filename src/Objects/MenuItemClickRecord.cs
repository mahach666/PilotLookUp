using System;

namespace PilotLookUp.Objects
{
    // Клик по пункту меню плагина
    public sealed class MenuItemClickRecord
    {
        public MenuItemClickRecord(string itemName)
        {
            ItemName = itemName;
            DateTime = DateTime.Now;
        }

        public string ItemName { get; }
        public DateTime DateTime { get; }
    }
}
