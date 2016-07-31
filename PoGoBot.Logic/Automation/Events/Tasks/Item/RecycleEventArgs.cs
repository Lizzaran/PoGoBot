using System;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Logic.Automation.Events.Tasks.Item
{
    public class RecycleEventArgs : EventArgs
    {
        public RecycleEventArgs(RecycleInventoryItemResponse response, ItemId itemId, int amount)
        {
            Response = response;
            ItemId = itemId;
            Amount = amount;
        }

        public RecycleInventoryItemResponse Response { get; }
        public ItemId ItemId { get; }
        public int Amount { get; }
    }
}