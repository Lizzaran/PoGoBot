using POGOLib.Net;
using POGOProtos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoGoBot.Logic.Extensions;
using POGOProtos.Inventory.Item;

namespace PoGoBot.Logic.Automation.Filters.Pokemon
{
    public class EvolveLuckyEggActiveFilter : BaseFilter<IEnumerable<PokemonData>>
    {
        public EvolveLuckyEggActiveFilter(Settings settings, Session session, Func<bool> enabledFunction) : base(settings, session, enabledFunction)
        {
        }

        public override IEnumerable<PokemonData> Process(IEnumerable<PokemonData> input)
        {
            var items = Session.Player.Inventory.GetAppliedItems();
            return input.Where(p => (Session.Player.Inventory.GetAmountOfItem(ItemId.ItemLuckyEgg) == 0) || 
                Session.Player.Inventory.IsItemActive(ItemType.XpBoost)).ToList();
        }
    }
}
