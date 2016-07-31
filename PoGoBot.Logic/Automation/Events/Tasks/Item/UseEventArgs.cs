using System;
using POGOProtos.Inventory.Item;

namespace PoGoBot.Logic.Automation.Events.Tasks.Item
{
    public class UseEventArgs : EventArgs
    {
        public UseEventArgs(ItemId itemId, int amount, bool success)
        {
            ItemId = itemId;
            Amount = amount;
            Success = success;
        }

        public ItemId ItemId { get; }
        public int Amount { get; }
        public bool Success { get; }
    }
}