using System.Collections.Generic;
using System.Linq;
using PoGoBot.Logic.Automation.Events.Tasks.Item;
using PoGoBot.Logic.Automation.Filters.Item;
using PoGoBot.Logic.Extensions;
using PoGoBot.Logic.Helpers;
using PoGoBot.Logic.Interfaces;
using POGOProtos.Inventory.Item;

namespace PoGoBot.Logic.Automation.Tasks.Item
{
    public class RecycleTask : BaseTask
    {
        private readonly Pipeline<IEnumerable<ItemData>> _pipeline;

        public RecycleTask(Context context) : base(context)
        {
            _pipeline = new Pipeline<IEnumerable<ItemData>>();
            _pipeline.Register(new List<IFilter<IEnumerable<ItemData>>>
            {
                new RecycleWhitelistFilter(Context.Settings, Context.Session)
            });
        }

        public override bool Enabled => Context.Settings.Bot.Item.Recycle.Enabled;

        public override void OnStart()
        {
        }

        public override void OnExecute()
        {
            var items = _pipeline.Execute(Context.Session.Player.Inventory.GetItems());
            foreach (var item in items)
            {
                var maxAmount =
                    Context.Settings.Bot.Item.Recycle.Items.First(i => i.ItemId == item.ItemId).MaximumAmount;
                var dropAmount = item.Count - maxAmount;
                if (dropAmount > 0)
                {
                    var recycleResponse = Context.RpcRequest.Item.Recycle(item.ItemId, dropAmount);
                    Context.Events.DispatchEvent(this, new RecycleEventArgs(recycleResponse, item.ItemId, dropAmount));
                }
            }
        }

        public override void OnTerminate()
        {
        }
    }
}