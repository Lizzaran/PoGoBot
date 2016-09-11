using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGoBot.Logic.Automation.Events.Tasks.Item
{
    public class UseItemEggIncubatorArgs : EventArgs
    {
        public UseItemEggIncubatorArgs(UseItemEggIncubatorResponse response, string itemId, ulong pokemonId)
        {
            Response = response;
            PokemonId = pokemonId;
            ItemId = itemId;



        }

        public UseItemEggIncubatorResponse Response { get; }
        public ulong PokemonId { get; }
        public string ItemId { get; }
    }
}
