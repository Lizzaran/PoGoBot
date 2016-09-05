using PoGoBot.Logic.Automation.Events.Tasks.Player;
using PoGoBot.Logic.Extensions;
using POGOProtos.Inventory.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGoBot.Logic.Automation.Tasks.Player
{
    class UseLuckyEggTask : BaseTask
    {

        public UseLuckyEggTask(Context context) : base(context)
        {
        }

        public override bool Enabled => Context.Settings.Bot.UseLuckyEgg.Enabled;

        public override void OnExecute()
        {
            if(! Context.Session.Player.Inventory.IsItemActive(ItemType.XpBoost) &&
                Context.Session.Player.Inventory.GetAmountOfItem(ItemId.ItemLuckyEgg) > 0)
            {
                if ((Context.Session.Player.Inventory.GetPokemons().Count + Context.Session.Player.Inventory.GetEggs().Count) * Context.Settings.Bot.UseLuckyEgg.PercentStorageFull >= 100 * Context.Session.Player.Data?.MaxPokemonStorage )
                {
                    var useLuckyEggResponse = Context.RpcRequest.Player.UseLuckyEgg();
                    Context.Events.DispatchEvent(this, new UseLuckyEggEventArgs(useLuckyEggResponse));
                }
            }
        }

        public override void OnStart()
        {
            
        }

        public override void OnTerminate()
        {

        }

    }
}
