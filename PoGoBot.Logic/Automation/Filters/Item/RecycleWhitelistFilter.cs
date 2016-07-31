using System.Collections.Generic;
using System.Linq;
using POGOLib.Net;
using POGOProtos.Inventory.Item;

namespace PoGoBot.Logic.Automation.Filters.Item
{
    public class RecycleWhitelistFilter : BaseFilter<IEnumerable<ItemData>>
    {
        public RecycleWhitelistFilter(Settings settings, Session session) : base(settings, session)
        {
        }

        public override IEnumerable<ItemData> Process(IEnumerable<ItemData> input)
        {
            return input.Where(i => i != null && Settings.Bot.Item.Recycle.Items.Any(x => x.ItemId == i.ItemId));
        }
    }
}