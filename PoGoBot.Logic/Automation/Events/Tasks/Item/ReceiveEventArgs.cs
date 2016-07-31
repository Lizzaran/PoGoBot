using System;
using System.Collections.Generic;
using POGOProtos.Inventory.Item;

namespace PoGoBot.Logic.Automation.Events.Tasks.Item
{
    public class ReceiveEventArgs : EventArgs
    {
        public ReceiveEventArgs(Dictionary<ItemId, int> items)
        {
            Items = items;
        }

        public Dictionary<ItemId, int> Items { get; }
    }
}